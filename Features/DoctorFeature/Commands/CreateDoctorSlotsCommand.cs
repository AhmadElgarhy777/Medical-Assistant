using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Features.DoctorFeature.Commands
{
    public record CreateDoctorSlotsCommand(
        string DoctorId,
        DayOfWeek Day,
        string FromTime,
        string ToTime
    ) : IRequest<bool>;
}
