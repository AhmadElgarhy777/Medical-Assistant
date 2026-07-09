using Models.Models;

namespace Features.NotifecationService.NotificationSender
{
    public interface INotificationSender
    {
        Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
    }
}
