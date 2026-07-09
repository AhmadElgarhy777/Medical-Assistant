using MediatR;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Features;

namespace Features.LabFeature.Commands
{

    //public class RegisterLabCommand : IRequest<ResultResponse<string>>
    //{
    //    public string Name { get; set; } = null!;
    //    public string Email { get; set; } = null!;
    //    public string Password { get; set; } = null!;
    //    public string Address { get; set; } = null!;
    //    public string Phone { get; set; } = null!;
    //    public string AreaId { get; set; } = null!;
    //    public string LabLicense { get; set; } = null!;
    //}

    public class UpdateLabProfileCommand : IRequest<ResultResponse<string>>
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string AreaId { get; set; } = null!;
    }

    public class UpdateLabWorkingHoursCommand : IRequest<ResultResponse<string>> { public string WorkingHours { get; set; } = null!; }
    
    public class UpdateLabLocationCommand : IRequest<ResultResponse<string>> 
    { 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public record UpdateLabLogoCommand(IFormFile File) : IRequest<ResultResponse<string>>;

    public class AddLabTestCommand : IRequest<ResultResponse<string>>
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public string EstimatedTime { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
    }

    public class UpdateLabTestCommand : AddLabTestCommand
    { 
        public string Id { get; set; } = null!; 
    }

    public record DeleteLabTestCommand(string Id) : IRequest<ResultResponse<string>>;

    public record ChangeLabBookingStatusCommand(string Id, Models.Enums.LabBookingStatusEnum Status) : IRequest<ResultResponse<string>>;

    public record UploadLabResultCommand(string Id, List<IFormFile> Files, string? JsonResult, string? DoctorNotes) : IRequest<ResultResponse<string>>;
    public record DeleteLabResultCommand(string Id) : IRequest<ResultResponse<string>>;

    public class AddLabScheduleCommand : IRequest<ResultResponse<string>>
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public class UpdateLabScheduleCommand : AddLabScheduleCommand
    { 
        public string Id { get; set; } = null!; 
    }

    public record DeleteLabScheduleCommand(string Id) : IRequest<ResultResponse<string>>;

    public record MarkLabNotificationReadCommand(string Id) : IRequest<ResultResponse<string>>;

    public class BookLabTestCommand : IRequest<ResultResponse<string>>
    {
        public string LabId { get; set; } = null!;
        public Models.Enums.VisitTypeEnum VisitType { get; set; }
        public List<string> TestIds { get; set; } = new List<string>();
        public string? HomeAddress { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string ScheduledTimeSlot { get; set; } = null!;
    }

    public record CancelPatientLabBookingCommand(string Id) : IRequest<ResultResponse<string>>;

    // Home Collection specific
    public record AssignCollectorCommand(string Id, string CollectorId) : IRequest<ResultResponse<string>>;
    public record UpdateHomeCollectionStatusCommand(string Id, Models.Enums.LabBookingStatusEnum NewStatus) : IRequest<ResultResponse<string>>;
}
