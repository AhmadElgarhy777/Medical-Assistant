using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature;
using Features.AiFeature.SharedMethod;
using Features.AiFeature.SharedMethod.AiReportService;
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
        private readonly IAiReportService aiReportService;

        public SkinCancerClassificationCommandHandler(IImageService imageService,
            IAnalyzeImage analyzeImage,
            ISkinCancerClassificationAIClient skinCancerClassificationAIClient,
            IAiReportRepositry reportRepositry,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork,
            IValidator<SkinCancerClassificationCommand> validator,
            IAiReportService aiReportService

            )
        {
            this.imageService = imageService;
            this.analyzeImage = analyzeImage;
            this.skinCancerClassificationAIClient = skinCancerClassificationAIClient;
            this.reportRepositry = reportRepositry;
            this.reportImageRepository = reportImageRepository;
            this.unitOfWork = unitOfWork;
            this.validator = validator;
            this.aiReportService = aiReportService;
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
                    "SkinCancerModel",
                    cancellationToken);

                // 2- Analyze Images
                var analysisResult =
                  await aiReportService.AnalyzeAndSaveAsync(
                      uploadedImages,
                      AiModelTypeEnum.SkinCancerClassification,
                      request.PatientId,
                      request.DoctorId,
                      request.DoctorNote,
                      cancellationToken);
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = true,
                    Message = "Analysis completed successfully.",
                    Obj = analysisResult.AnalysisResult
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

        
    }
}