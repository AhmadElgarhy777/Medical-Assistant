using Features;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NotifecationService.Commands.MarkAsRead
{
    public record MarkAsReadCommand(string NotificationId, string UserId):IRequest<ResultResponse<bool>>;


}
