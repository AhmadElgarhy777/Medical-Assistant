using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature;
using Features.AiFeature.CBCBloodTest;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SharedMethod;
using Features.AiFeature.SkinCancerClassification;
using Features.PatientMedicalScanFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Models;
using Models.Enums;

namespace Features.PatientMedicalScanFeature.Handlers
{
    public class AnalyzePatientScanCommandHandler : IRequestHandler<AnalyzePatientScanCommand, ResultResponse<AiReport>>
    {
        private readonly IPatientMedicalScanRepositry _scanRepository;
        private readonly IAnalyzeImage _analyzeImage;
        private readonly IBrainTumorAIClient _brainTumorClient;
        private readonly ISkinCancerClassificationAIClient _skinCancerClient;
        private readonly IChestRayClassifcationAiClient _chestRayClient;
        private readonly ICBCBloodTestAiClient _cbcClient;
        private readonly IAiReportRepositry _reportRepository;
        private readonly IAiReportImageRepositry _reportImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public AnalyzePatientScanCommandHandler(
            IPatientMedicalScanRepositry scanRepository,
            IAnalyzeImage analyzeImage,
            IBrainTumorAIClient brainTumorClient,
            ISkinCancerClassificationAIClient skinCancerClient,
            IChestRayClassifcationAiClient chestRayClient,
            ICBCBloodTestAiClient cbcClient,
            IAiReportRepositry reportRepository,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            _scanRepository = scanRepository;
            _analyzeImage = analyzeImage;
            _brainTumorClient = brainTumorClient;
            _skinCancerClient = skinCancerClient;
            _chestRayClient = chestRayClient;
            _cbcClient = cbcClient;
            _reportRepository = reportRepository;
            _reportImageRepository = reportImageRepository;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<ResultResponse<AiReport>> Handle(AnalyzePatientScanCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch scan request
            var scan = await _scanRepository.GetTable()
                .FindAsync(new object[] { request.ScanId }, cancellationToken);

            if (scan == null)
            {
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = "Patient scan not found."
                };
            }

            if (scan.DoctorId != request.DoctorId)
            {
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = "Unauthorized. You are not the assigned doctor for this scan."
                };
            }

            if (scan.Status != MedicalScanStatusEnum.PendingDoctorReview)
            {
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = $"Scan is already in '{scan.Status}' state and cannot be analyzed."
                };
            }

            var absoluteImagePath = Path.Combine(_environment.WebRootPath, scan.ImagePath);
            if (!File.Exists(absoluteImagePath))
            {
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = "The saved scan image file could not be found on the server."
                };
            }

            // 2. Select appropriate AI Client based on ModelType
            IAIModelClient aiClient = scan.ModelType switch
            {
                AiModelTypeEnum.BrainTumorDetection => _brainTumorClient,
                AiModelTypeEnum.SkinCancerClassification => _skinCancerClient,
                AiModelTypeEnum.ChestRayClassifcation => _chestRayClient,
                AiModelTypeEnum.CBCBloodTest => _cbcClient,
                _ => throw new ArgumentOutOfRangeException(nameof(scan.ModelType), "Unknown AI Model Type.")
            };

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // 3. Trigger prediction on AI microservice
                var analysisResult = await _analyzeImage.AnalyzeImagesAsync(
                    new List<string> { absoluteImagePath },
                    aiClient,
                    cancellationToken
                );

                // 4. Create AiReport
                var report = new AiReport
                {
                    Diagnosis = analysisResult.Prediction,
                    Confidence = analysisResult.Confidence,
                    ModelType = scan.ModelType,
                    PatientId = scan.PatientId,
                    DoctorId = scan.DoctorId,
                    DoctorNote = request.DoctorNote ?? scan.DoctorNote,
                    CreatedAt = DateTime.Now
                };
                _reportRepository.Add(report);

                // 5. Save report image reference
                var extension = Path.GetExtension(scan.ImagePath).ToLowerInvariant();
                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "image/jpeg"
                };

                var reportImage = new AiReportImage
                {
                    ImagePath = scan.ImagePath,
                    AiReportId = report.ID,
                    ContentType = contentType,
                    UploadedAt = DateTime.Now
                };
                _reportImageRepository.Add(reportImage);

                // 6. Update scan request status and associate report
                scan.Status = MedicalScanStatusEnum.Analyzed;
                scan.AiReportId = report.ID;
                if (!string.IsNullOrEmpty(request.DoctorNote))
                {
                    scan.DoctorNote = request.DoctorNote;
                }

                _scanRepository.Edit(scan);

                await _unitOfWork.CompleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<AiReport>
                {
                    ISucsses = true,
                    Message = "AI analysis completed successfully. Report generated.",
                    Obj = report
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = $"An error occurred during AI analysis: {ex.Message}",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
