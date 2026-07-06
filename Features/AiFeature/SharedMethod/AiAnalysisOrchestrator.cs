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
using Features.AiFeature.SkinCancerClassification;
using Models;
using Models.Enums;

namespace Features.AiFeature.SharedMethod
{
    public class AiAnalysisOrchestrator : IAiAnalysisOrchestrator
    {
        private readonly IAnalyzeImage _analyzeImage;
        private readonly IBrainTumorAIClient _brainTumorClient;
        private readonly ISkinCancerClassificationAIClient _skinCancerClient;
        private readonly IChestRayClassifcationAiClient _chestRayClient;
        private readonly ICBCBloodTestAiClient _cbcClient;
        private readonly IAiReportRepositry _reportRepository;
        private readonly IAiReportImageRepositry _reportImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AiAnalysisOrchestrator(
            IAnalyzeImage analyzeImage,
            IBrainTumorAIClient brainTumorClient,
            ISkinCancerClassificationAIClient skinCancerClient,
            IChestRayClassifcationAiClient chestRayClient,
            ICBCBloodTestAiClient cbcClient,
            IAiReportRepositry reportRepository,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork)
        {
            _analyzeImage = analyzeImage;
            _brainTumorClient = brainTumorClient;
            _skinCancerClient = skinCancerClient;
            _chestRayClient = chestRayClient;
            _cbcClient = cbcClient;
            _reportRepository = reportRepository;
            _reportImageRepository = reportImageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<AiReport>> ProcessAnalysisAsync(
            List<string> imagePaths,
            string patientId,
            string doctorId,
            string? doctorNote,
            AiModelTypeEnum modelType,
            CancellationToken cancellationToken)
        {
            if (imagePaths == null || imagePaths.Count == 0)
            {
                return new ResultResponse<AiReport>
                {
                    ISucsses = false,
                    Message = "No images provided for analysis."
                };
            }

            // 1. Select appropriate AI Client based on ModelType
            IAIModelClient aiClient = modelType switch
            {
                AiModelTypeEnum.BrainTumorDetection => _brainTumorClient,
                AiModelTypeEnum.SkinCancerClassification => _skinCancerClient,
                AiModelTypeEnum.ChestRayClassifcation => _chestRayClient,
                AiModelTypeEnum.CBCBloodTest => _cbcClient,
                _ => throw new ArgumentOutOfRangeException(nameof(modelType), "Unknown AI Model Type.")
            };

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // 2. Trigger prediction on AI microservice
                var analysisResult = await _analyzeImage.AnalyzeImagesAsync(
                    imagePaths,
                    aiClient,
                    cancellationToken
                );

                // 3. Create AiReport
                var report = new AiReport
                {
                    Diagnosis = analysisResult.Prediction,
                    Confidence = analysisResult.Confidence,
                    ModelType = modelType,
                    PatientId = patientId,
                    DoctorId = doctorId,
                    DoctorNote = doctorNote,
                    CreatedAt = DateTime.Now
                };
                _reportRepository.Add(report);

                // 4. Save report images references
                foreach (var path in imagePaths)
                {
                    var extension = Path.GetExtension(path).ToLowerInvariant();
                    var contentType = extension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".webp" => "image/webp",
                        _ => "image/jpeg"
                    };

                    _reportImageRepository.Add(new AiReportImage
                    {
                        ImagePath = path,
                        AiReportId = report.ID,
                        ContentType = contentType,
                        UploadedAt = DateTime.Now
                    });
                }

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
                    Errors = new List<string> { ex.InnerException?.ToString() ?? ex.Message }
                };
            }
        }
    }
}
