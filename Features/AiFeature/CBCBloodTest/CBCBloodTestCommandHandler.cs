using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SharedMethod;
using Features.AiFeature.SharedMethod.AiReportService;
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
        private readonly IAiReportService aiReportService;

        public CBCBloodTestCommandHandler(IImageService imageService,
            IAnalyzeImage analyzeImage,
            ICBCBloodTestAiClient cBCBloodTestAiClient,
            IAiReportRepositry reportRepositry,
            IAiReportImageRepositry reportImageRepository,
            IUnitOfWork unitOfWork,
            IAiReportService aiReportService

            )
        {
            this.imageService = imageService;
            this.analyzeImage = analyzeImage;
            this.cBCBloodTestAiClient = cBCBloodTestAiClient;
            this.reportRepositry = reportRepositry;
            this.reportImageRepository = reportImageRepository;
            this.unitOfWork = unitOfWork;
            this.aiReportService = aiReportService;
        }
        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(CBCBloodTestCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 1- Upload Images
                var uploadedImages = await imageService.UploadAIModelImagesAsync(
                    request.Images,
                    "CBCBloodTestModel",
                    cancellationToken);

                // 2- Analyze Images
                var analysisResult =
                  await aiReportService.AnalyzeAndSaveAsync(
                      uploadedImages,
                      AiModelTypeEnum.CBCBloodTest,
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