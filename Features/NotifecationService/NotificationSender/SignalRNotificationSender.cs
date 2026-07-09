using Microsoft.AspNetCore.SignalR;
using Models.Models;

namespace Features.NotifecationService.NotificationSender
{
    public class SignalRNotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationSender(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients
                .User(notification.ReceiverId)
                .SendAsync("ReceiveNotification", new
                {
                    notification.ID,
                    notification.Title,
                    notification.Body,
                    notification.Type,
                    notification.ReferenceId,
                    notification.ReferenceType,
                    notification.CreatedAt
                }, cancellationToken);
        }
    }
}
