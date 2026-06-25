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
    public class UnBanNurseCommandHandler : IRequestHandler<UnBanNurseCommand, ResultResponse<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INuresRepositry nuresRepositry;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnBanNurseCommandHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            INuresRepositry nuresRepositry,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            this.nuresRepositry = nuresRepositry;
            _userManager = userManager;
        }

        public async Task<ResultResponse<bool>> Handle(UnBanNurseCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the nurse
            var spec = new NurseSpesfication(n=>n.ID==request.NurseId&&n.IsDeleted==true);
            var nurse = nuresRepositry.GetOne(spec).FirstOrDefault();

            if (nurse == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "nurse not found.",
                    Obj = false
                };

            if (!nurse.IsDeleted)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "nurse is already Banned.",
                    Obj = false
                };


            // 2. Find user account
            var user = await _userManager.FindByIdAsync(request.NurseId);
            if (user == null)
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = "User not found.",
                    Obj = false
                };

            nurse.IsDeleted = false;
            nurse.DeletedAT = null;

          

            // 5. Unban the user account
            var unbanResult = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!unbanResult.Succeeded)
            {
                return new ResultResponse<bool>
                {

                    ISucsses = false,
                    Message = "Failed to unban user account.",
                    Obj = false
                };
            }


            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 3. Restore the nurse

                // 6. Reset failed access count
                await _userManager.ResetAccessFailedCountAsync(user);

                // 7. Refresh security stamp so they can login again
                await _userManager.UpdateSecurityStampAsync(user);

                // 8. Save changes
                nuresRepositry.Edit(nurse);
                await _unitOfWork.CompleteAsync(cancellationToken);

                await _mediator.Publish(new UnBannedUserEvent(
                   nurse.Email,
                   nurse.UserName
               ), cancellationToken);

                // 9. Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // 10. Publish event to notify user by email

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
                    Message = $"An error occurred while deleting the nurse: {ex.Message} : inner exception {ex.InnerException?.ToString()}",
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
