using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
using Features.NotifecationService.Commands.CreateNotifcation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs.AiServicesDtos;
using Models.Enums;
using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.CreateScanRequest
{
    public class CreateScanRequestCommandHandler
     : IRequestHandler<CreateScanRequestCommand, ResultResponse<string>>
    {
        private readonly IScanRequestRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IValidator<CreateScanRequestCommand> validator;

        public CreateScanRequestCommandHandler(
            IScanRequestRepository repository,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
           IValidator<CreateScanRequestCommand> validator
)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
            this.validator = validator;
        }

        public async Task<ResultResponse<string>> Handle(CreateScanRequestCommand request, CancellationToken cancellationToken)
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
            var doctorId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (doctorId is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message="The Doctor Invalid"
                };
            }

            var patient = await userManager.FindByIdAsync(request.PatientId);

            if (patient is null)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "The Patient Invalid"
                };

            var exists = await repository
                        .GetTable()
                        .AnyAsync(x =>
                        x.PatientId == request.PatientId &&
                        x.DoctorId == doctorId &&
                        x.AIModelType == request.AIModelType &&
                        x.Status != ScanRequestStatus.Completed &&
                        x.Status != ScanRequestStatus.Cancelled &&
                        x.Status != ScanRequestStatus.Expired,
                        cancellationToken);

            if (exists)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "There is already an active request"
                };

            var scanRequest = new ScanRequest
            {
                PatientId = request.PatientId,
                DoctorId = doctorId,
                AIModelType = request.AIModelType,
                DoctorNote = request.DoctorNote,
                ExpirationDate = request.ExpirationDate,
                Status = ScanRequestStatus.Pending
                
            };

            repository.Add(scanRequest);

            await repository.CommitAsync(cancellationToken);

            await mediator.Send(new CreateNotificationCommand(
                ReceiverId: request.PatientId,
                SenderId: doctorId,
                Title: "New Scan Request",
                Body: $"Your doctor requested {request.AIModelType} images.",
                Type: NotificationTypeEnum.ScanRequestedToPatient,
                ReferenceType: NotificationReferenceType.ScanRequest,
                ReferenceId: scanRequest.ID.ToString()
            ), cancellationToken);


            return new ResultResponse<string>
            {
                ISucsses = true,
                Obj = scanRequest.ID
            };
        }
    }
}
