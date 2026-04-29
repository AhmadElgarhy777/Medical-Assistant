using Features.SuperAdminFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Handler
{
    public class UpdateSuperAdminHandler : IRequestHandler<UpdateSuperAdminCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UpdateSuperAdminHandler(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse<string>> Handle(
            UpdateSuperAdminCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Get the current logged-in Super Admin from JWT ────────────────
            var userId = httpContextAccessor.HttpContext?.User
                            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Fail("Unauthorized.");

            var superAdmin = await userManager.FindByIdAsync(userId);
            if (superAdmin == null)
                return Fail("User not found.");

            // ── 2. Verify current password before allowing any change ────────────
            var passwordValid = await userManager.CheckPasswordAsync(superAdmin, request.CurrentPassword);
            if (!passwordValid)
                return Fail("Current password is incorrect.");

            var errors = new List<string>();

            // ── 3. Update Email (if provided) ────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.NewEmail) &&
                request.NewEmail != superAdmin.Email)
            {
                var emailTaken = await userManager.FindByEmailAsync(request.NewEmail);
                if (emailTaken != null)
                    return Fail("This email is already in use.");

                var emailToken = await userManager.GenerateChangeEmailTokenAsync(superAdmin, request.NewEmail);
                var emailResult = await userManager.ChangeEmailAsync(superAdmin, request.NewEmail, emailToken);

                if (!emailResult.Succeeded)
                    errors.AddRange(emailResult.Errors.Select(e => e.Description));
            }

            // ── 4. Update UserName (if provided) ─────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.NewUserName) &&
                request.NewUserName != superAdmin.UserName)
            {
                var userNameResult = await userManager.SetUserNameAsync(superAdmin, request.NewUserName);
                if (!userNameResult.Succeeded)
                    errors.AddRange(userNameResult.Errors.Select(e => e.Description));
            }

            // ── 5. Update Password (if provided) ─────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                if (request.NewPassword != request.ConfirmNewPassword)
                    return Fail("New passwords do not match.");

                var passResult = await userManager.ChangePasswordAsync(
                                     superAdmin,
                                     request.CurrentPassword,
                                     request.NewPassword);

                if (!passResult.Succeeded)
                    errors.AddRange(passResult.Errors.Select(e => e.Description));
            }

            // ── 6. Return result ─────────────────────────────────────────────────
            if (errors.Any())
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Some updates failed.",
                    Errors = errors
                };

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Super Admin updated successfully."
            };
        }

        private static ResultResponse<string> Fail(string message) =>
            new() { ISucsses = false, Message = message };
    }
}
