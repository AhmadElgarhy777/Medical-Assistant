using Features;
using MediatR;
using Models.Enums.NotificationEnums;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Commands.CreateNotifcation
{
    public record CreateNotificationCommand(string ReceiverId,
        string? SenderId,
        string Title, string Body,
        NotificationTypeEnum Type,
        NotificationReferenceType ReferenceType,
        string? ReferenceId):IRequest<ResultResponse<string>>;


}
