using Features;
using GraduationProject_MedicalAssistant_.NotifecationService.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Queries.GetNotifcation
{
    public record GetNotificationsQuery(string UserId):IRequest<ResultResponse<List<NotificationDto>>>;
    
}
