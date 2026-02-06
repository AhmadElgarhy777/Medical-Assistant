using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Models;
using Services.EmailServices;
using Services.OTPConfirmServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Events.Handlers
{
    public class ConfirmEmailEventHAndler : INotificationHandler<ConfirmEmailEvent>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailServices emailServices;
        private readonly IOTPConfirmEmailService oTPConfirm;

        public ConfirmEmailEventHAndler(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailServices emailServices,
            IOTPConfirmEmailService oTPConfirm)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailServices = emailServices;
            this.oTPConfirm = oTPConfirm;
        }
        public async Task Handle(ConfirmEmailEvent notification, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(notification.Email);
            if (user != null)
            {
                if (notification.Type == "link")
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var ConfirmLink = $"{configuration["ConfirmationEmailFrontEndUrl"]}/VerifyEmail?UserId={notification.UserId}&Token={encodedToken}";

                    await emailServices.SendEmailAsync(notification.Email,
                        "Confirm Your Email.....! ",
                        $"Your Link For Confirm Your Email is : \n {ConfirmLink} \n please use it for confirm the email....\n thank you....",
                        cancellationToken);
                }
                else
                {
                   await oTPConfirm.SendAsync(notification.UserId, notification.Email,cancellationToken);
                }
            }
           


        }
    }
}
