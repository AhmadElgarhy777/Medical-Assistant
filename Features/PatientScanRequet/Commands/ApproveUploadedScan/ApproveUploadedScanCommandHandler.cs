using DataAccess.Repositry.IRepositry;
using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
using Features.NotifecationService.Commands.CreateNotifcation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.ApproveUploadedScan
{
    public class ApproveUploadedScanCommandHandler
    : IRequestHandler<ApproveUploadedScanCommand, ResultResponse<string>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMediator mediator;
        private readonly IValidator<ApproveUploadedScanCommand> validator;

        public ApproveUploadedScanCommandHandler(
            IScanRequestRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IValidator<ApproveUploadedScanCommand> validator
)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
            this.validator = validator;
        }

        public async Task<ResultResponse<string>> Handle(
            ApproveUploadedScanCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Validation failed.",
                    Errors = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList()
                };
            }
            var doctorId = httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (doctorId is null)
                return new()
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };

            var scanRequest = await repository
                .GetTable()
                .FirstOrDefaultAsync(x =>
                    x.ID == request.ScanRequestId,
                    cancellationToken);

            if (scanRequest is null)
                return new()
                {
                    ISucsses = false,
                    Message = "Scan request not found."
                };

            if (scanRequest.DoctorId != doctorId)
                return new()
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };

            if (scanRequest.Status != ScanRequestStatus.Uploaded)
                return new()
                {
                    ISucsses = false,
                    Message = "Patient didn't upload images yet."
                };

            scanRequest.Status = ScanRequestStatus.Approved;
            scanRequest.RejectReason = null;

            await repository.CommitAsync(cancellationToken);

            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: scanRequest.PatientId,
                SenderId: doctorId,
                Title: "Images Approved",
                Body: "Your uploaded medical images have been approved.",
                Type: NotificationTypeEnum.ScanRequestApproved,
                ReferenceType: NotificationReferenceType.ScanRequest,
                ReferenceId: scanRequest.ID
            ), cancellationToken);

            return new()
            {
                ISucsses = true,
                Obj = scanRequest.ID
            };
        }
    }
}
