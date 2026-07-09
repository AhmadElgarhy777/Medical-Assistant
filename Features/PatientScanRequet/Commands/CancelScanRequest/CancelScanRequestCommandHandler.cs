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

namespace Features.PatientScanRequet.Commands.CancelScanRequest
{
    public class CancelScanRequestCommandHandler
         : IRequestHandler<CancelScanRequestCommand, ResultResponse<string>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMediator mediator;

        public CancelScanRequestCommandHandler(
            IScanRequestRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        public async Task<ResultResponse<string>> Handle(
            CancelScanRequestCommand request,
            CancellationToken cancellationToken)
        {
            var doctorId = httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (doctorId is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };
            }

            var scanRequest = await repository
                .GetTable()
                .FirstOrDefaultAsync(x =>
                    x.ID == request.ScanRequestId,
                    cancellationToken);

            if (scanRequest is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Scan request not found."
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

            if (scanRequest.Status is ScanRequestStatus.Completed
                or ScanRequestStatus.Analyzed
                or ScanRequestStatus.Cancelled
                or ScanRequestStatus.Expired)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = $"Cannot cancel request in '{scanRequest.Status}' status."
                };
            }

            scanRequest.Status = ScanRequestStatus.Cancelled;
            scanRequest.CancelReason = request.CancelReason;

            await repository.CommitAsync(cancellationToken);

            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: scanRequest.PatientId,
                SenderId: doctorId,
                Title: "Scan Request Cancelled",
                Body: "The doctor cancelled your scan request.",
                Type: NotificationTypeEnum.ScanRequestCancelledByDoctor,
                ReferenceType: NotificationReferenceType.ScanRequest,
                ReferenceId: scanRequest.ID
            ), cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Scan request cancelled successfully.",
                Obj = scanRequest.ID
            };
        }
    }
}
