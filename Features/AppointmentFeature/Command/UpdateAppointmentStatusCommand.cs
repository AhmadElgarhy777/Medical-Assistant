using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Enums;

namespace Features.AppointmentFeature.Commands
{
    // بنبعت رقم الحجز والحالة الجديدة اللي عاوزين نوصل لها
    public record UpdateAppointmentStatusCommand(string AppointmentId, bookStatusEnum NewStatus) : IRequest<bool>;
}
