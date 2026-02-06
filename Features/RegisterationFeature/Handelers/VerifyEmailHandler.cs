using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public VerifyEmailHandler(UserManager<ApplicationUser > userManager )
        {
            this.userManager = userManager;
        }
        public async Task<ResultResponse<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var userId= request.UserId;
            var token = request.Token;

            var user=await userManager.FindByIdAsync( userId );
            if (user is not null)
            {

                if (user.EmailConfirmed)
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = true,
                        Message = "The Email Is Already Confirmed , Try Login...!"
                    };
                }
                var tokenDecoded=Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

                var confirmUser=await userManager.ConfirmEmailAsync(user, tokenDecoded);

                if (confirmUser.Succeeded)
                {
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
                        Message = "The Email Is Not Confirmed , Try Again Later..."
                    };
                }

            }

            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The User Is Not Found,...."
            };


        }
    }
}
