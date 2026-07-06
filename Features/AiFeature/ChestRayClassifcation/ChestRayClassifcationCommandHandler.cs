using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.SharedMethod;
using Features.AiFeature.SkinCancerClassification;
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

namespace Features.AiFeature.ChestRayClassifcation
{
    internal class ChestRayClassifcationCommandHandler : IRequestHandler<ChestRayClassifcationCommand, ResultResponse<AiAnalysisResultDTO>>
    {
        private readonly IImageService imageService;
        private readonly IAiAnalysisOrchestrator orchestrator;
        private readonly IValidator<ChestRayClassifcationCommand> validator;

        public ChestRayClassifcationCommandHandler(
            IImageService imageService,
            IAiAnalysisOrchestrator orchestrator,
            IValidator<ChestRayClassifcationCommand> validator)
        {
            this.imageService = imageService;
            this.orchestrator = orchestrator;
            this.validator = validator;
        }

        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(ChestRayClassifcationCommand request, CancellationToken cancellationToken)
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

            try
            {
                // 1- Upload Images
                var uploadedImages = await imageService.UploadAIModelImagesAsync(
                    request.Images,
                    cancellationToken);

                // 2- Process analysis via Orchestrator
                var orchestratorResult = await orchestrator.ProcessAnalysisAsync(
                    uploadedImages,
                    request.PatientId,
                    request.DoctorId,
                    request.DoctorNote,
                    AiModelTypeEnum.ChestRayClassifcation,
                    cancellationToken);

                if (!orchestratorResult.ISucsses)
                {
                    return new ResultResponse<AiAnalysisResultDTO>
                    {
                        ISucsses = false,
                        Message = orchestratorResult.Message,
                        Errors = orchestratorResult.Errors
                    };
                }

                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = true,
                    Message = "Analysis completed successfully.",
                    Obj = new AiAnalysisResultDTO
                    {
                        Prediction = orchestratorResult.Obj.Diagnosis,
                        Confidence = orchestratorResult.Obj.Confidence
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = $"An error occurred during analysis: {ex.Message}",
                    Errors = new List<string> { ex.InnerException?.ToString() ?? ex.Message }
                };
            }
        }
    }
}
