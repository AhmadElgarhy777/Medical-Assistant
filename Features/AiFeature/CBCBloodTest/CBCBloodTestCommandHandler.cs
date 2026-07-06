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
        private readonly IAiAnalysisOrchestrator orchestrator;

        public CBCBloodTestCommandHandler(
            IImageService imageService,
            IAiAnalysisOrchestrator orchestrator)
        {
            this.imageService = imageService;
            this.orchestrator = orchestrator;
        }

        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(CBCBloodTestCommand request, CancellationToken cancellationToken)
        {
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
                    AiModelTypeEnum.CBCBloodTest,
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
