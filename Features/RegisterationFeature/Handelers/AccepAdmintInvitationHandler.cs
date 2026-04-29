using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class AccepAdmintInvitationHandler : IRequestHandler<AccepAdmintInvitationCommand, ResultResponse<string>>
    {
       
        private readonly UserManager<ApplicationUser> userManager;

        public AccepAdmintInvitationHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ResultResponse<string>> Handle(AccepAdmintInvitationCommand request, CancellationToken cancellationToken)
        {
            // ── 1. Validate passwords match ──────────────────────────────────────
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Passwords do not match."
                };
            }

            // ── 2. Find user ─────────────────────────────────────────────────────
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Invalid invitation link."
                };
            }

            // ── 3. Decode tokens ─────────────────────────────────────────────────
            var decodedEmailToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(request.EmailToken));

            var decodedPasswordToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(request.PasswordToken));

            // ── 4. Confirm email ─────────────────────────────────────────────────
            var confirmResult = await userManager.ConfirmEmailAsync(user, decodedEmailToken);
            if (!confirmResult.Succeeded)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Email confirmation failed. The link may have expired."
                };
            }

            // ── 5. Set the admin's own password ──────────────────────────────────
            var resetResult = await userManager.ResetPasswordAsync(user, decodedPasswordToken, request.NewPassword);
            if (!resetResult.Succeeded)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Failed to set password.",
                    Errors = resetResult.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Your account is ready. You can now log in."
            };
        }
    }
}

