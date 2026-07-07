using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SharedMethod;
using MediatR;
using Models;
using Models.DTOs.AiServicesDtos;
using Services.ImageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.CBCBloodTest
{
    public class CBCBloodTestCommandHandler : IRequestHandler<CBCBloodTestCommand, ResultResponse<AiAnalysisResultDTO>>
    {
        private readonly IImageService imageService;
        private readonly IAnalyzeImage analyzeImage;
        private readonly ICBCBloodTestAiClient cBCBloodTestAiClient;
        private readonly IAiReportRepositry reportRepositry;
        private readonly IAiReportImageRepositry reportImageRepository;
        private readonly IUnitOfWork unitOfWork;

        public CBCBloodTestCommandHandler(IImageService imageService,
            IAnalyzeImage analyzeImage,
            ICBCBloodTestAiClient cBCBloodTestAiClient,
            IAiReportRepositry reportRepositry,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork

            )
        {
            this.imageService = imageService;
            this.analyzeImage = analyzeImage;
            this.cBCBloodTestAiClient = cBCBloodTestAiClient;
            this.reportRepositry = reportRepositry;
            this.reportImageRepository = reportImageRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(CBCBloodTestCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 1- Upload Images
                var uploadedImages = await imageService.UploadAIModelImagesAsync(
                    request.Images,
                    cancellationToken);

                // 2- Analyze Images
                var analysisResult = await analyzeImage.AnalyzeImagesAsync(
                    uploadedImages,
                    cBCBloodTestAiClient,
                    cancellationToken);

                // 3- Create Report
                var Report = new AiReport
                {
                    Diagnosis = analysisResult.Prediction,
                    Confidence = analysisResult.Confidence,
                    ModelType = AiModelTypeEnum.CBCBloodTest,
                    PatientId = request.PatientId,
                    DoctorId = request.DoctorId,
                    DoctorNote = request.DoctorNote,
                };
                // 4- Save Report
                reportRepositry.Add(Report);
                // 5- Save Report Images

                SaveImages(Report.ID, uploadedImages);

                await unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = true,
                    Message = "Analysis completed successfully.",
                    Obj = analysisResult
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = $"An error occurred during analysis.,{ex.Message}",
                    Errors = new List<string> { ex.InnerException?.ToString() ?? ex.Message }
                };
            }


        }

        private void SaveImages(string reportId, List<string> imagePaths)
        {
            foreach (var path in imagePaths)
            {
                var extension = System.IO.Path.GetExtension(path).ToLowerInvariant();
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
                    ImagePath = path,
                    AiReportId = reportId,
                    ContentType = contentType
                });
            }
        }
    }
}