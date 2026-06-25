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

    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, ResultResponse<bool>>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDoctorRepositry doctorRepositry;
        private readonly UserManager<ApplicationUser> userManager;

        public DeleteDoctorCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IDoctorRepositry doctorRepositry, UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.doctorRepositry = doctorRepositry;
            this.userManager = userManager;
        }

        public async Task<ResultResponse<bool>> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the Doc
            var spec = new DoctorSpecifcation(request.DoctorId);
            var Doc = doctorRepositry.GetOne(spec).FirstOrDefault();

            if (Doc == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Doctor not found.",
                    Obj = false
                };


            if (Doc.IsDeleted)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Doctor is already deleted.",
                    Obj = false
                };
            var user = await userManager.FindByIdAsync(request.DoctorId);
            if (user == null )
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "User not found.",
                    Obj = false
                };

            // 2. Soft delete instead of hard delete
            var deletedAt = DateTime.Now;

            // 2. Soft delete the Doc
            Doc.IsDeleted = true;
            Doc.DeletedAT = deletedAt;

            // 3. Soft delete all related entities
            SoftDeleteRange(Doc.Appointments, deletedAt);
            SoftDeleteRange(Doc.Prescriptions, deletedAt);
            SoftDeleteRange(Doc.avilableTimes, deletedAt);
            SoftDeleteRange(Doc.DoctorPatients, deletedAt);

            await userManager.SetLockoutEnabledAsync(user, true);
            var lockResult = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (!lockResult.Succeeded)
            {

                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "Failed to lock the user account.",
                    Obj = false
                };
            }

            using var transactions = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
               
                // 5. Revoke all active sessions
                await userManager.UpdateSecurityStampAsync(user);
                doctorRepositry.Edit(Doc);

                // 3. Save changes
                await unitOfWork.CompleteAsync(cancellationToken);
                await transactions.CommitAsync(cancellationToken);
                await mediator.Publish(new UserBannedEvent(Doc.Email, Doc.UserName), cancellationToken);

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
                    Message = $"An error occurred while deleting the Doc: {ex.Message} : inner exception {ex.InnerException?.ToString()}",
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
