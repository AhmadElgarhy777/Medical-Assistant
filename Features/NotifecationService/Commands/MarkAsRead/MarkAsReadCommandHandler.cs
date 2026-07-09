using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NotifecationService.Commands.MarkAsRead
{
    public class MarkAsReadCommandHandler
     : IRequestHandler<MarkAsReadCommand, ResultResponse<bool>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsReadCommandHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<bool>> Handle(
            MarkAsReadCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _notificationRepository
                .GetTable()
                .FirstOrDefaultAsync(x =>
                    x.ID == request.NotificationId &&
                    x.ReceiverId == request.UserId,
                    cancellationToken);

                if (notification == null)
                    throw new Exception("Notification not found.");

                notification.IsRead = true;

                _notificationRepository.Edit(notification);

                await _notificationRepository.CommitAsync(cancellationToken);
            }catch (Exception ex)
            {
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = $"{ex.Message} , InnerException : {ex.InnerException.ToString()}",
                };

            }

            return new ResultResponse<bool>
            {
                ISucsses = true,
               Obj=true,
            };



        }
    }
}
