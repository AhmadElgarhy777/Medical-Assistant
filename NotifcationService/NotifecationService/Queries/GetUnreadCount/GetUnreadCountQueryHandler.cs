using DataAccess.Repositry.IRepositry;
using Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Queries.GetUnreadCount
{
    public class GetUnreadCountQueryHandler
    : IRequestHandler<GetUnreadCountQuery, ResultResponse<int>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUnreadCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ResultResponse<int>> Handle(
            GetUnreadCountQuery request,
            CancellationToken cancellationToken)
        {
            var count= await _notificationRepository
                .GetTable()
                .CountAsync(x =>
                    x.ReceiverId == request.UserId &&
                    !x.IsRead,
                    cancellationToken);

            return new ResultResponse<int>
            {
                ISucsses = true,
                Obj = count,
            };
        }
    }
}
