using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature;
using Features.AiFeature.SharedMethod;
using FluentValidation;
using MediatR;
using Models;
using Models.DTOs.AiServicesDtos;
using Services.ImageServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SkinCancerClassification
{
    public class SkinCancerClassificationCommandHandler : IRequestHandler<SkinCancerClassificationCommand, ResultResponse<AiAnalysisResultDTO>>
    {
        private readonly IImageService imageService;
        private readonly IAnalyzeImage analyzeImage;
        private readonly ISkinCancerClassificationAIClient skinCancerClassificationAIClient;
        private readonly IAiReportRepositry reportRepositry;
        private readonly IAiReportImageRepositry reportImageRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IValidator<SkinCancerClassificationCommand> validator;

        public SkinCancerClassificationCommandHandler(IImageService imageService,
            IAnalyzeImage analyzeImage,
            ISkinCancerClassificationAIClient skinCancerClassificationAIClient,
            IAiReportRepositry reportRepositry,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork,
            IValidator<SkinCancerClassificationCommand> validator

            )
        {
            this.imageService = imageService;
            this.analyzeImage = analyzeImage;
            this.skinCancerClassificationAIClient = skinCancerClassificationAIClient;
            this.reportRepositry = reportRepositry;
            this.reportImageRepository = reportImageRepository;
            this.unitOfWork = unitOfWork;
            this.validator = validator;
        }
        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(SkinCancerClassificationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = "Validation failed.",
                    Errors = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList()
                };
            }
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
                    skinCancerClassificationAIClient,
                    cancellationToken);

                // 3- Create Report
                var Report = new AiReport
                {
                    Diagnosis = analysisResult.Prediction,
                    Confidence = analysisResult.Confidence,
                    ModelType = AiModelTypeEnum.SkinCancerClassification,
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
