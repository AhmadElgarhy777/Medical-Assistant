using Features;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.NotifecationService.Queries.GetUnreadCount
{
    public record GetUnreadCountQuery(string UserId):IRequest<ResultResponse<int>>;
    
}
