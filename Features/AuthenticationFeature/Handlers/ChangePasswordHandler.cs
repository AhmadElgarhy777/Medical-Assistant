using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Events.Event;
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

namespace Features.AuthenticationFeature.Handlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResultResponse<String>>
    {
        private readonly IHttpContextAccessor http;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMediator mediator;

        public ChangePasswordHandler(IHttpContextAccessor http,UserManager<ApplicationUser> userManager,IMediator mediator)
        {
            this.http = http;
            this.userManager = userManager;
            this.mediator = mediator;
        }
        public async Task<ResultResponse<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var OldPass=request.PasswordDTO.OldPassword;
            var NewPass=request.PasswordDTO.NewPassword;
            var userId=http?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                var result=await userManager.ChangePasswordAsync(user,OldPass,NewPass);
                if (result.Succeeded)
                {
                    await mediator.Publish(new ChangePasswordMessageEvent(userId, user.Email,cancellationToken));
                    return new ResultResponse<string>
                    {
                        ISucsses = true,
                        Message = "The Password Changed Succefully."
                    };

                }
                else
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = false,
                        Message = "The Password Not Changed , Try Again....",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The User Is Not Found Try Login Again...."
            };

        }
    }
}
