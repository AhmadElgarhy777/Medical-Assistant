using Features.AuthenticationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using Services.OTPConfirmServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    internal class ForgetPasswordViaOtpCommandHandler : IRequestHandler<ForgetPasswordViaOtpCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOTPConfirmEmailService otpService;

        public ForgetPasswordViaOtpCommandHandler(
            UserManager<ApplicationUser> userManager,
            IOTPConfirmEmailService otpService)
        {
            this.userManager = userManager;
            this.otpService = otpService;
        }
        public async Task<ResultResponse<string>> Handle(ForgetPasswordViaOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = $"The Email {request.Email} Is Not Found."
                };
            }

            await otpService.SendAsync(
                user.Id,
                user.Email!,
                "Reset Password OTP",
                "Your OTP for resetting your password is:",
                cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = $"An OTP has been sent to {user.Email}."
            };
        }
    }
}
