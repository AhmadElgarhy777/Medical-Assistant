using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Features.RadiologyFeature.Commands
{
    public record CreateRadiologyBookingCommand(
        string RadiologyCenterId,
        DateTime ScheduledDate,
        string ScheduledTimeSlot,
        List<string> RadiologyScanIds,
        string PaymentMethod
    ) : IRequest<ResultResponse<string>>;
}