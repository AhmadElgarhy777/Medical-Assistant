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
    public class UserBannedEventHandler : INotificationHandler<UserBannedEvent>
    {
        private readonly IEmailServices _emailService;

        public UserBannedEventHandler(IEmailServices emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(UserBannedEvent notification, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAsync(
            
                to: notification.Email,
                Subject: "Your account has been suspended",
                body: $"""
                Dear {notification.Name},

                Your account has been suspended due to a violation of our terms.
                If you believe this is a mistake, please contact support.

                Regards,
                The Support Team
                """
            );
        }
    }
}
