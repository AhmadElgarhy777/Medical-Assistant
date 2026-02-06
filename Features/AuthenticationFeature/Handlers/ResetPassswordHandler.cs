using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class ResetPassswordHandler : IRequestHandler<ResetPassswordCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly IMediator mediator;

        public ResetPassswordHandler(UserManager<ApplicationUser> userManger,IMediator mediator)
        {
            this.userManger = userManger;
            this.mediator = mediator;
        }
        public async Task<ResultResponse<string>> Handle(ResetPassswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManger.FindByIdAsync(request.ResetDto.UserId);
            if (user is not null)
            {
                var decodedToken=Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetDto.Token));
                var result=await userManger.ResetPasswordAsync(user, decodedToken, request.ResetDto.NewPassword);
                if (result.Succeeded) 
                {
                    await mediator.Publish(new ChangePasswordMessageEvent(request.ResetDto.UserId, user.Email, cancellationToken));
                    return new ResultResponse<string>
                    {
                        ISucsses = true,
                        Message = "The Password Was Reset Succesfully \n show your email for more information"
                    };
                
                }
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "The Password Was Not Reset, TryAgain ...!",
                    Errors = result.Errors.Select(e => e.Description).ToList()

                };

            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The User Is Not Found.....!"
            };
        }
    }
}
