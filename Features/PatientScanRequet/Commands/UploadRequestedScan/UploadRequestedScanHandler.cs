using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
using Features.NotifecationService.Commands.CreateNotifcation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
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

namespace Features.PatientScanRequet.Commands.UploadRequestedScan
{
    public class UploadRequestedScanCommandHandler
     : IRequestHandler<UploadRequestedScanCommand, ResultResponse<string>>
    {
        private readonly IRequestScanImageRepositry requestScanImageRepositry;
        private readonly IScanRequestRepository repository;
        private readonly IImageService imageService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMediator mediator;
        private readonly IValidator<UploadRequestedScanCommand> validator;

        public UploadRequestedScanCommandHandler(
            IRequestScanImageRepositry requestScanImageRepositry ,
            IScanRequestRepository repository,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IValidator<UploadRequestedScanCommand> validator
)
        {
            this.requestScanImageRepositry = requestScanImageRepositry;
            this.repository = repository;
            this.imageService = imageService;
            this.httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
            this.validator = validator;
        }

        public async Task<ResultResponse<string>> Handle(
            UploadRequestedScanCommand request,
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
            var patientId = httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (patientId is null)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Patient Invlaid"
                };


            var scanRequest = await repository
                .GetTable()
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.ID == request.ScanRequestId, cancellationToken);

            if (scanRequest is null)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Scan request not found."
                };

            if (scanRequest.PatientId != patientId)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Unauthorized"
                };

            if (scanRequest.Status != ScanRequestStatus.Pending && scanRequest.Status != ScanRequestStatus.Rejected)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "This request is closed"
                };
            if (scanRequest.ExpirationDate.HasValue &&
                scanRequest.ExpirationDate.Value < DateTime.UtcNow)
            {
                scanRequest.Status = ScanRequestStatus.Expired;

                await repository.CommitAsync(cancellationToken);
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "This request has expired."
                }; 
            }

            if (request.Images is null || !request.Images.Any())
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Can not insert null images."
                };

            }

           

            List<string> uploadedImages = new();

            try
            {
                if (scanRequest.Images.Any())
                {
                    await imageService.DeleteAIImagesAsync(
                        scanRequest.Images.Select(x => x.ImagePath));

                    requestScanImageRepositry.DeleteRange(scanRequest.Images);
                }

                uploadedImages =
                await imageService.UploadAIModelImagesAsync(
                    request.Images,
                    "RequestedScanImage",
                    cancellationToken);

            foreach (var image in uploadedImages)
            {
                scanRequest.Images.Add(new RequestedScanImage
                {
                    ImagePath = image,
                    ScanRequestId = scanRequest.ID
                });
            }

            scanRequest.Status = ScanRequestStatus.Uploaded;

            await repository.CommitAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                if (uploadedImages.Any())
                    await imageService.DeleteAIImagesAsync(uploadedImages);

                return new ResultResponse<string>
                {
                    ISucsses = false,
                   Message= ex.Message
                };
            }

            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: scanRequest.DoctorId,
                SenderId: patientId,
                Title: "Images Uploaded",
                Body: "The patient has uploaded the requested medical images.",
                Type: NotificationTypeEnum.ScanImagesForRequestUploaded,
                ReferenceType: NotificationReferenceType.ScanRequest,
                ReferenceId: scanRequest.ID 
            ), cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Obj = scanRequest.ID
            };
        }
    }
}
