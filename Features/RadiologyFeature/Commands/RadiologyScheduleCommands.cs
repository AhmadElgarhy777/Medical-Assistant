using MediatR;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Features;

namespace Features.RadiologyFeature.Commands
{
    public record AddRadiologyScheduleCommand : IRequest<ResultResponse<string>>
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public record UpdateRadiologyScheduleCommand : IRequest<ResultResponse<string>> 
    { 
        public string Id { get; set; } = null!;
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public record DeleteRadiologyScheduleCommand(string Id) : IRequest<ResultResponse<string>>;
}
