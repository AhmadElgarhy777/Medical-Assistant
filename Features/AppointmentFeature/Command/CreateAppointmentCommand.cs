using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Enums;

namespace Features.AppointmentFeature.Commands
{
    // بنقول لـ MediatR إن ده "أمر" وبيرجع "bool" (نجح ولا فشل)
    public record CreateAppointmentCommand (string DoctorId, string SlotId, BookTypeEnum Type) 
        : IRequest<ResultResponse<String>>;
    
}
