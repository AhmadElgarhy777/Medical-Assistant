using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class ForgetPassswordHandler : IRequestHandler<ForgetPassswordCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMediator mediator;

        public ForgetPassswordHandler(UserManager<ApplicationUser> userManager,IMediator mediator)
        {
            this.userManager = userManager;
            this.mediator = mediator;
        }
        public async Task<ResultResponse<string>> Handle(ForgetPassswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is not null)
            {
                var token=await userManager.GeneratePasswordResetTokenAsync(user);
                await mediator.Publish(new ForgetPasswordEvent(user.Id, user.Email, token, cancellationToken));
                return new ResultResponse<string>
                {
                    ISucsses = true,
                    Message = $"The Link For Reset Was Sent To Your Email \n {user.Email}"
                };
            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = $"The Email {request.Email} Is Not Found ... Invaild Email"
            };
        }
    }
}
