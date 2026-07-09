using DataAccess.Repositry.IRepositry;
using Features.NotifecationService.Commands.CreateNotifcation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.PublishAiReport
{
    public class PublishAiReportCommandHandler
        : IRequestHandler<PublishAiReportCommand, ResultResponse<string>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMediator mediator;

        public PublishAiReportCommandHandler(
            IScanRequestRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        public async Task<ResultResponse<string>> Handle(
            PublishAiReportCommand request,
            CancellationToken cancellationToken)
        {
            var doctorId = httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var scanRequest = await repository
                .GetTable()
                .Include(x => x.AiReport)
                .FirstOrDefaultAsync(x =>
                    x.ID == request.ScanRequestId,
                    cancellationToken);

            if (scanRequest is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Scan Request Not Found."
                };
            }

            if (scanRequest.DoctorId != doctorId)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };
            }

            if (scanRequest.Status != ScanRequestStatus.Analyzed)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Scan Request is not analyzed yet."
                };
            }

            if (scanRequest.AiReport is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "AI Report not found."
                };
            }

            // تحديث ملاحظة الدكتور النهائية
            scanRequest.AiReport.DoctorNote = request.DoctorNote;

            // إنهاء الطلب
            scanRequest.Status = ScanRequestStatus.Completed;

            await repository.CommitAsync(cancellationToken);

            // إشعار للمريض
            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: scanRequest.PatientId,
                SenderId: doctorId,
                Title: "Medical Report Ready",
                Body: "Your medical report has been reviewed and is now available.",
                Type: NotificationTypeEnum.AIReportPublishedCompeletlly,
                ReferenceType: NotificationReferenceType.AiReport,
                ReferenceId: scanRequest.AiReport.ID
            ), cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Report published successfully.",
                Obj = scanRequest.AiReport.ID
            };
        }
    }
}
