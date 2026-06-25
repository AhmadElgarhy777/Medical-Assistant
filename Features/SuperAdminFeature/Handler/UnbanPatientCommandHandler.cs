using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.SuperAdminFeature.Command;
using Features.SuperAdminFeature.Events.events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Handler
{
    public class UnbanPatientCommandHandler : IRequestHandler<UnbanPatientCommand, ResultResponse<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepositry _patientRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnbanPatientCommandHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IPatientRepositry patientRepository,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
            _userManager = userManager;
        }

        public async Task<ResultResponse<bool>> Handle(UnbanPatientCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the patient
            var spec = new PatientSpecifcation(p=>p.ID==request.PatientId&&p.IsDeleted==true);
            var patient = _patientRepository.GetOne(spec).FirstOrDefault();

            if (patient == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Patient not found.",
                    Obj = false
                };

            if (!patient.IsDeleted)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Patient is already Banned.",
                    Obj = false
                };


            // 2. Find user account
            var user = await _userManager.FindByIdAsync(request.PatientId);
            if (user == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "User not found.",
                    Obj = false
                };

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 3. Restore the patient
                patient.IsDeleted = false;
                patient.DeletedAT = null;

                // 4. Restore all related entities
                RestoreRange(patient.appointments);
                RestoreRange(patient.DoctorPatients);
                RestoreRange(patient.Prescriptions);
                RestoreRange(patient.AiReports);
                RestoreRange(patient.patientPhones);

                // 5. Unban the user account
                var unbanResult = await _userManager.SetLockoutEndDateAsync(user, null);
                if (!unbanResult.Succeeded)
                    return new ResultResponse<bool>
                    {
                        ISucsses = false,
                        Message = "Failed to unban user account.",
                        Obj = false
                    };

                // 6. Reset failed access count
                await _userManager.ResetAccessFailedCountAsync(user);

                // 7. Refresh security stamp so they can login again
                await _userManager.UpdateSecurityStampAsync(user);

                // 8. Save changes
                _patientRepository.Edit(patient);
                await _unitOfWork.CompleteAsync(cancellationToken);

                // 9. Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // 10. Publish event to notify user by email
                await _mediator.Publish(new UnBannedUserEvent(
                    patient.Email,
                    patient.UserName
                ), cancellationToken);

                return new ResultResponse<bool>
                {
                    ISucsses = true,
                    Obj = true
                };
            }
            catch (Exception ex)
            {
                if (transaction.GetDbTransaction().Connection != null)
                    await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = $"An error occurred while deleting the patient: {ex.Message} : inner exception {ex.InnerException?.ToString()}",
                    Obj = false
                };
            }
        }

        private static void RestoreRange<T>(IEnumerable<T>? collection)
            where T : ModelBase
        {
            if (collection == null) return;
            foreach (var item in collection)
            {
                item.IsDeleted = false;
                item.DeletedAT = null;
            }
        }
    }
}
