using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features;
using GraduationProject_MedicalAssistant_.Hubs;
using GraduationProject_MedicalAssistant_.NotifecationService.NotificationSender;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Commands.CreateNotifcation
{
    public class CreateNotificationCommandHandler
         : IRequestHandler<CreateNotificationCommand, ResultResponse<string>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly INotificationSender notificationSender;
        private readonly ILogger logger;

        public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository,
             IHubContext<NotificationHub> hubContext,
             INotificationSender notificationSender,
             ILogger logger
            )
        {
            _notificationRepository = notificationRepository;
            this.hubContext = hubContext;
            this.notificationSender = notificationSender;
            this.logger = logger;
        }

        public async Task<ResultResponse<string>> Handle(
            CreateNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                ReceiverId = request.ReceiverId,
                SenderId = request.SenderId,
                Title = request.Title,
                Body = request.Body,
                Type = request.Type,
                ReferenceType = request.ReferenceType,
                ReferenceId = request.ReferenceId
            };

            _notificationRepository.Add(notification);

            await _notificationRepository.CommitAsync(cancellationToken);

            await hubContext
                .Clients
                .User(request.ReceiverId)
                .SendAsync(
                       "ReceiveNotification",
                         new
                         {
                             notification.ID,
                             notification.Title,
                             notification.Body,
                             notification.Type,
                             notification.ReferenceId,
                             notification.ReferenceType,
                             notification.CreatedAt
                         },
                       cancellationToken);
            try
            {
                await notificationSender.SendAsync(notification);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification via SignalR.");
            }
            return new ResultResponse<string>
            {
                ISucsses = true,
                Obj = notification.ID,
            };
        }
    }
}
