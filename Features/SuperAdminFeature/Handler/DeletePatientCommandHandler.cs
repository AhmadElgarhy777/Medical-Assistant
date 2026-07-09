using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.SuperAdminFeature.Command;
using Features.SuperAdminFeature.Events.events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Models;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Handler
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, ResultResponse<bool>>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPatientRepositry patientRepositry;
        private readonly UserManager<ApplicationUser> userManager;

        public DeletePatientCommandHandler(IMediator mediator,IUnitOfWork unitOfWork,IPatientRepositry patientRepositry, UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.patientRepositry = patientRepositry;
            this.userManager = userManager;
        }

        public async Task<ResultResponse<bool>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the patient
            var spec = new PatientSpecifcation(request.PatientId);
            var patient = patientRepositry.GetOne(spec).FirstOrDefault();

            if (patient == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Patient not found.",
                    Obj = false
                };


            if (patient.IsDeleted)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Patient is already deleted.",
                    Obj = false
                };
            var user = await userManager.FindByIdAsync(request.PatientId);
            if (user == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "User not found.",
                    Obj = false
                };

            // 2. Soft delete instead of hard delete
            var deletedAt = DateTime.Now;

            // 2. Soft delete the patient
            patient.IsDeleted = true;
            patient.DeletedAT = deletedAt;
            patient.BanCount += 1;

            // 3. Soft delete all related entities
            SoftDeleteRange(patient.appointments, deletedAt);
            SoftDeleteRange(patient.DoctorPatients, deletedAt);
            SoftDeleteRange(patient.Prescriptions, deletedAt);
            SoftDeleteRange(patient.AiReports, deletedAt);
            SoftDeleteRange(patient.patientPhones, deletedAt);

            await userManager.SetLockoutEnabledAsync(user, true);
            var lockResult = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (!lockResult.Succeeded)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Failed to lock the user account.",
                    Obj = false
                };

           using  var transactions = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                
                // 5. Revoke all active sessions
                await userManager.UpdateSecurityStampAsync(user);
                patientRepositry.Edit(patient);

                // 3. Save changes
                await unitOfWork.CompleteAsync();
                await transactions.CommitAsync(cancellationToken);

                await mediator.Publish(new UserBannedEvent(patient.Email,patient.UserName), cancellationToken);
                return new ResultResponse<bool>
                {
                    ISucsses = true,
                    Obj = true
                };

            }
            catch (Exception ex)
            {
                if (transactions.GetDbTransaction().Connection != null)
                    await transactions.RollbackAsync(cancellationToken);
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = $"An error occurred while deleting the patient: {ex.Message} : inner exception {ex.InnerException?.ToString()}",
                    Obj = false
                };
            }


           
        }
        private static void SoftDeleteRange<T>(IEnumerable<T>? collection, DateTime deletedAt)
           where T : ModelBase
        {
            if (collection == null) return;
            foreach (var item in collection)
            {
                item.IsDeleted = true;
                item.DeletedAT = deletedAt;
            }
        }
    }
}
