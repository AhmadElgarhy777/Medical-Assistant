using Features.SuperAdminFeature.Events.events;
using MediatR;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Events.Handlers
{
    internal class UserUnBannedEventHandler : INotificationHandler<UnBannedUserEvent>
    {
        private readonly IEmailServices _emailService;

        public UserUnBannedEventHandler(IEmailServices emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(UnBannedUserEvent notification, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAsync(

                to: notification.Email,
                Subject: "Your account has been suspended",
                body: $"""
                Dear {notification.Name},

                Your account has been restored and you can now login again.
                If you have any questions, please contact support.

                Regards,
                The Support Team
                """
            );
        }
    }
}
