using DataAccess.Repositry.IRepositry;
using Features.NotifecationService.Commands.CreateNotifcation;
using Features.PatientScanRequet.Commands.ApproveUploadedScan;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Enums.NotificationEnums;
using Services.ImageServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.RejectUploadedScan
{
    public class RejectUploadedScanCommandHandler
     : IRequestHandler<RejectUploadedScanCommand, ResultResponse<string>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IRequestScanImageRepositry imageRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageService imageService;
        private readonly IMediator mediator;
        private readonly IValidator<RejectUploadedScanCommand> validator;

        public RejectUploadedScanCommandHandler(
            IScanRequestRepository repository,
            IRequestScanImageRepositry imageRepository,
            IHttpContextAccessor httpContextAccessor,
            IImageService imageService,
            IMediator mediator,
            IValidator<RejectUploadedScanCommand> validator
)
        {
            this.repository = repository;
            this.imageRepository = imageRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.imageService = imageService;
            this.mediator = mediator;
            this.validator = validator;
        }

        public async Task<ResultResponse<string>> Handle(
            RejectUploadedScanCommand request,
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
                .Include(x => x.Images)
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

            try
            {
                if (scanRequest.Images.Any())
                {
                    await imageService.DeleteAIImagesAsync(
                        scanRequest.Images.Select(x => x.ImagePath));

                    imageRepository.DeleteRange(scanRequest.Images);
                }

                scanRequest.Status = ScanRequestStatus.Rejected;
                scanRequest.RejectReason = request.RejectReason;

                await repository.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new()
                {
                    ISucsses = false,
                    Message = ex.Message
                };
            }

            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: scanRequest.PatientId,
                SenderId: doctorId,
                Title: "Images Rejected",
                Body: $"Your uploaded images were rejected.\nReason: {request.RejectReason}, Please Upload agian....",
                Type: NotificationTypeEnum.ScanRequestRejected,
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
