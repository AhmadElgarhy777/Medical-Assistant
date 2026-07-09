using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
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
using Twilio.TwiML.Voice;

namespace Features.AiFeature.AnalyzeBrainTumorFeature.Handlers
{
    internal class AnalyzeBrainTumorCommandHandler : IRequestHandler<AnalyzeBrainTumorCommand, ResultResponse<AiAnalysisResultDTO>>
    {
        private readonly IImageService imageService;
        private readonly IAnalyzeImage analyzeImage;
        private readonly IBrainTumorAIClient brainTumorAIClient;
        private readonly IAiReportRepositry reportRepositry;
        private readonly IAiReportImageRepositry reportImageRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IValidator<AnalyzeBrainTumorCommand> validator;
        private readonly IAiReportService aiReportService;

        public AnalyzeBrainTumorCommandHandler(IImageService imageService,
            IAnalyzeImage analyzeImage,
            IBrainTumorAIClient brainTumorAIClient,
            IAiReportRepositry reportRepositry,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork,
            IValidator<AnalyzeBrainTumorCommand> validator,
            IAiReportService aiReportService

            )
        {
            this.imageService = imageService;
            this.analyzeImage = analyzeImage;
            this.brainTumorAIClient = brainTumorAIClient;
            this.reportRepositry = reportRepositry;
            this.reportImageRepository = reportImageRepository;
            this.unitOfWork = unitOfWork;
            this.validator = validator;
            this.aiReportService = aiReportService;
        }
        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(AnalyzeBrainTumorCommand request, CancellationToken cancellationToken)
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
                    "BrainTumorModel"
                    ,cancellationToken);

                var analysisResult =
                 await aiReportService.AnalyzeAndSaveAsync(
                     uploadedImages,
                     AiModelTypeEnum.BrainTumorDetection,
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