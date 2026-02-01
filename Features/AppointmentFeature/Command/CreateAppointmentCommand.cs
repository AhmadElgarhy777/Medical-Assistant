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
    public class CreateAppointmentCommand : IRequest<bool>
    {
        public string DoctorId { get; set; }
        public string SlotId { get; set; }
        public BookTypeEnum Type { get; set; } // مثلاً كشف في العيادة أو أونلاين
    }
}
