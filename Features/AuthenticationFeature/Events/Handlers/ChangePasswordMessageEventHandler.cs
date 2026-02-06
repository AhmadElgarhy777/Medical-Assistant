using Features.AuthenticationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Events.Handlers
{
    public class ChangePasswordMessageEventHandler : INotificationHandler<ChangePasswordMessageEvent>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailServices emailServices;

        public ChangePasswordMessageEventHandler(UserManager<ApplicationUser> userManager,IEmailServices emailServices)
        {
            this.userManager = userManager;
            this.emailServices = emailServices;
        }
        public async Task Handle(ChangePasswordMessageEvent notification, CancellationToken cancellationToken)
        {
            var user =userManager.FindByIdAsync(notification.UserId);
            if(user is not null)
            {
                await emailServices.SendEmailAsync(notification.Email,
                    "Change Password Alarm",
                    $"You Change Your Password Now At\n {DateTime.UtcNow.ToString()} \n Have AGood Day");
            }


        }
    }
}
