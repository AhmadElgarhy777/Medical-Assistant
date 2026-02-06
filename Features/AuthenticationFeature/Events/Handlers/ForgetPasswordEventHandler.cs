using Features.AuthenticationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Events.Handlers
{
    public class ForgetPasswordEventHandler : INotificationHandler<ForgetPasswordEvent>
    {
        private readonly IEmailServices emailServices;
        private readonly IConfiguration configuration;

        public ForgetPasswordEventHandler(IEmailServices emailServices,IConfiguration configuration)
        {
            this.emailServices = emailServices;
            this.configuration = configuration;
        }
        public async Task Handle(ForgetPasswordEvent notification, CancellationToken cancellationToken)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.Token));
            var Link = $"{configuration["ResetPasswordFrontEndUrl"]}/ResetPassword?UserId={notification.UserId}&Token={encodedToken}";
            await emailServices.SendEmailAsync(notification.Email, "Reset You Password", $"Your Reset Link For Password Is \n {Link} ");
        
        }
    }
}
