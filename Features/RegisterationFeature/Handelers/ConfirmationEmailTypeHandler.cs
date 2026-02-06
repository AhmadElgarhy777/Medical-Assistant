using Features.RegisterationFeature.Commands;
using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class ConfirmationEmailTypeHandler : IRequestHandler<ConfirmationEmailTypeCommand, ResultResponse<String>>
    {
        private readonly IMediator mediator;
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmationEmailTypeHandler(IMediator mediator,UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator;
            this.userManager = userManager;
        }
        public async Task<ResultResponse<string>> Handle(ConfirmationEmailTypeCommand request, CancellationToken cancellationToken)
        {
            var userId=request.TypeDTO.UserId;
            var confirmType = request.TypeDTO.Type;

            var user =await userManager.FindByIdAsync(userId);
            if(user is not null) 
            {
                if (user.EmailConfirmed)
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = true,
                        Message = "The Email Is Already Confirmed , Try Login...!"
                    };
                }
                await mediator.Publish(new ConfirmEmailEvent(userId, user.Email,cancellationToken, confirmType.ToString()));
                return new ResultResponse<string>
                {
                    ISucsses =true,
                    Message = $"The Confirmation Is Sent To {user.Email}"
                };

            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = $"The user is not register yet...!"
            };

        }
    }
}
