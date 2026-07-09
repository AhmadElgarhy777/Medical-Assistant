using Models.Models;

namespace GraduationProject_MedicalAssistant_.NotifecationService.NotificationSender
{
    public interface INotificationSender
    {
        Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
    }
}
