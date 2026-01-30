using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Features.AppointmentFeature.Commands
{
    public record CancelAppointmentCommand(string AppointmentId) : IRequest<bool>;
}
