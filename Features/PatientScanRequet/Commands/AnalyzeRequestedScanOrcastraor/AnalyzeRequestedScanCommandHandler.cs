using DataAccess.Repositry.IRepositry;
using Features.AiFeature.SharedMethod.AiReportService;
using Features.NotifecationService.Commands.CreateNotifcation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.AiServicesDtos;
using Models.Enums;
using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.AnalyzeRequestedScanOrcastraor
{
    public class AnalyzeRequestedScanCommandHandler
         : IRequestHandler<AnalyzeRequestedScanCommand, ResultResponse<AiAnalysisResultDTO>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IAiReportService aiReportService;
        private readonly IMediator mediator;

        public AnalyzeRequestedScanCommandHandler(
            IScanRequestRepository repository,
            IAiReportService aiReportService,
            IMediator mediator)
        {
            this.repository = repository;
            this.aiReportService = aiReportService;
            this.mediator = mediator;
        }

        public async Task<ResultResponse<AiAnalysisResultDTO>> Handle(
            AnalyzeRequestedScanCommand request,
            CancellationToken cancellationToken)
        {
            var scanRequest = await repository
                .GetTable()
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.ID == request.ScanRequestId,
                    cancellationToken);

            if (scanRequest is null)
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = "Scan request not found."
                };
            }

            if (scanRequest.Status != ScanRequestStatus.Approved)
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = "Images must be approved first."
                };
            }

            if (!scanRequest.Images.Any())
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = "No images found."
                };
            }

            var imagePaths = scanRequest.Images
                .Select(x => x.ImagePath)
                .ToList();

            try
            {
                var result = await aiReportService.AnalyzeAndSaveAsync(
                    imagePaths,
                    scanRequest.AIModelType,
                    scanRequest.PatientId,
                    scanRequest.DoctorId,
                    scanRequest.DoctorNote,
                    cancellationToken);

                scanRequest.AiReportId = result.ReportId;
                scanRequest.Status = ScanRequestStatus.Analyzed;

                await repository.CommitAsync(cancellationToken);

                await mediator.Send(
                    new CreateNotificationCommand(
                        ReceiverId: scanRequest.DoctorId,
                        SenderId: null,
                        Title: "AI Analysis Completed",
                        Body: "Medical image analysis has been completed successfully.",
                        Type: NotificationTypeEnum.AIReportGeneratedSuccessfully,
                        ReferenceType: NotificationReferenceType.AiReport,
                        ReferenceId: result.ReportId),
                    cancellationToken);

                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = true,
                    Message = "Analysis completed successfully.",
                    Obj = result.AnalysisResult
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<AiAnalysisResultDTO>
                {
                    ISucsses = false,
                    Message = ex.Message
                };
            }
        }
    }
}
