using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature;
using Features.AiFeature.CBCBloodTest;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SkinCancerClassification;
using Models;
using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod.AiReportService
{
    public class AiReportService : IAiReportService
    {
        private readonly IAnalyzeImage analyzeImage;

        private readonly IBrainTumorAIClient brainTumorAIClient;
        private readonly ISkinCancerClassificationAIClient skinCancerAIClient;
        private readonly IChestRayClassifcationAiClient chestRayAIClient;
        private readonly ICBCBloodTestAiClient cbcBloodTestAIClient;

        private readonly IAiReportRepositry reportRepository;
        private readonly IAiReportImageRepositry reportImageRepository;

        private readonly IUnitOfWork unitOfWork;

        public AiReportService(
            IAnalyzeImage analyzeImage,

            IBrainTumorAIClient brainTumorAIClient,
            ISkinCancerClassificationAIClient skinCancerAIClient,
            IChestRayClassifcationAiClient chestRayAIClient,
            ICBCBloodTestAiClient cbcBloodTestAIClient,

            IAiReportRepositry reportRepository,
            IAiReportImageRepositry reportImageRepository,

            IUnitOfWork unitOfWork)
        {
            this.analyzeImage = analyzeImage;

            this.brainTumorAIClient = brainTumorAIClient;
            this.skinCancerAIClient = skinCancerAIClient;
            this.chestRayAIClient = chestRayAIClient;
            this.cbcBloodTestAIClient = cbcBloodTestAIClient;

            this.reportRepository = reportRepository;
            this.reportImageRepository = reportImageRepository;

            this.unitOfWork = unitOfWork;
        }

        public async Task<AiReportResultDto> AnalyzeAndSaveAsync(
            List<string> imagePaths,
            AiModelTypeEnum modelType,
            string patientId,
            string? doctorId,
            string? doctorNote,
            CancellationToken cancellationToken)
        {
            var client = GetClient(modelType);

            using var transaction =
                await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var analysisResult =
                    await analyzeImage.AnalyzeImagesAsync(
                        imagePaths,
                        client,
                        cancellationToken);

                var report = new AiReport
                {
                    Diagnosis = analysisResult.Prediction,
                    Confidence = analysisResult.Confidence,
                    ModelType = modelType,
                    PatientId = patientId,
                    DoctorId = doctorId,
                    DoctorNote = doctorNote
                };

                reportRepository.Add(report);

                SaveImages(report.ID, imagePaths);

                await unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new AiReportResultDto{AnalysisResult=analysisResult,ReportId=report.ID }
                ;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private IAIModelClient GetClient(AiModelTypeEnum modelType)
        {
            return modelType switch
            {
                AiModelTypeEnum.BrainTumorDetection
                    => brainTumorAIClient,

                AiModelTypeEnum.SkinCancerClassification
                    => skinCancerAIClient,

                AiModelTypeEnum.ChestRayClassifcation
                    => chestRayAIClient,

                AiModelTypeEnum.CBCBloodTest
                    => cbcBloodTestAIClient,

                _ => throw new NotSupportedException("Unsupported AI Model")
            };
        }

        private void SaveImages(
            string reportId,
            List<string> imagePaths)
        {
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

                reportImageRepository.Add(new AiReportImage
                {
                    AiReportId = reportId,
                    ImagePath = path,
                    ContentType = contentType
                });
            }
        }
    }
}
