using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using Services.OTPConfirmServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class VerifyOTPHandler : IRequestHandler<VerifyOTPCommand, ResultResponse<String>>
    {
        private readonly IOTPConfirmEmailService otpServices;
        private readonly UserManager<ApplicationUser> userManager;

        public VerifyOTPHandler(IOTPConfirmEmailService otpServices,UserManager<ApplicationUser> userManager)
        {
            this.otpServices = otpServices;
            this.userManager = userManager;
        }
        public async Task<ResultResponse<string>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            var IsVerify=await otpServices.VerifyAsync(request.UserId, request.Otp, cancellationToken);
            if (IsVerify)
            {
                var user = await userManager.FindByIdAsync(request.UserId);
                if(user is not null)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                    return new ResultResponse<string>
                    {
                        ISucsses = true,
                        Message = "The Email Is Confirmed Succesfully"
                    };
                }
                else
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = false,
                        Message = "The User is Not Created yet...! ,\n Try Register Again...!"
                    };
                }
               
            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The Otp Is Invalid Please Try Agin...!"
            };
        }
    }
}
