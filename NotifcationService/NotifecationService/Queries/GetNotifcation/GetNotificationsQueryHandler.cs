using DataAccess.Repositry.IRepositry;
using Features;
using GraduationProject_MedicalAssistant_.NotifecationService.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Queries.GetNotifcation
{
    public class GetNotificationsQueryHandler
     : IRequestHandler<GetNotificationsQuery, ResultResponse<List<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ResultResponse<List<NotificationDto>>> Handle(
            GetNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var notifcations= await _notificationRepository
                .GetTable()
                .Where(x => x.ReceiverId == request.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationDto
                {
                    ID = x.ID,
                    Title = x.Title,
                    Body = x.Body,
                    Type = x.Type,
                    ReferenceType = x.ReferenceType,
                    ReferenceId = x.ReferenceId,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);


            return new ResultResponse<List<NotificationDto>>
            {
                ISucsses = true,
                Obj = notifcations
            };
        }
    }
}
