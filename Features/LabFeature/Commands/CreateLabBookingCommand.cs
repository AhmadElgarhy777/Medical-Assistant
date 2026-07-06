using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Models.Enums;

namespace Features.LabFeature.Commands
{
    public record CreateLabBookingCommand(
        VisitTypeEnum VisitType,
        string? LabId,
        string? AreaId,
        string? HomeAddress,
        DateTime ScheduledDate,
        string ScheduledTimeSlot,
        List<string> MedicalTestIds,
        string PaymentMethod
    ) : IRequest<ResultResponse<string>>;
}