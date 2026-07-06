using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Events.Event;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using Services.OTPConfirmServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class ResetPasswordByOtpHandler
        : IRequestHandler<ResetPasswordByOtpCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOTPConfirmEmailService otpService;
        private readonly IMediator mediator;
        private readonly IValidator<ResetPasswordByOtpCommand> validator;

        public ResetPasswordByOtpHandler(
            UserManager<ApplicationUser> userManager,
            IOTPConfirmEmailService otpService,
            IMediator mediator,
            IValidator<ResetPasswordByOtpCommand> validator)
        {
            this.userManager = userManager;
            this.otpService = otpService;
            this.mediator = mediator;
            this.validator = validator;
        }

        public async Task<ResultResponse<string>> Handle(
            ResetPasswordByOtpCommand request,
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

            var user = await userManager.FindByEmailAsync(request.ResetDto.Email);

            if (user is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "User not found."
                };
            }

            var isValidOtp = await otpService.VerifyAsync(
                user.Id,
                request.ResetDto.Otp,
                cancellationToken);

            if (!isValidOtp)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Invalid or expired OTP."
                };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(
                user,
                token,
                request.ResetDto.NewPassword);

            if (!result.Succeeded)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Password reset failed.",
                    Errors = result.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
            }

            await mediator.Publish(
                new ChangePasswordMessageEvent(
                    user.Id,
                    user.Email!,
                    cancellationToken));

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Password has been reset successfully."
            };
        }
    }
}
