using MediatR;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Features;

namespace Features.RadiologyFeature.Commands
{
    //public class RegisterRadiologyCommand : IRequest<ResultResponse<string>>
    //{
    //    public string Name { get; set; } = null!;
    //    public string Email { get; set; } = null!;
    //    public string Password { get; set; } = null!;
    //    public string Address { get; set; } = null!;
    //    public string Phone { get; set; } = null!;
    //    public string AreaId { get; set; } = null!;
    //}
    
    public class UpdateRadiologyProfileCommand : IRequest<ResultResponse<string>>
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string AreaId { get; set; } = null!;
    }

    public class UpdateRadiologyWorkingHoursCommand : IRequest<ResultResponse<string>> { public string WorkingHours { get; set; } = null!; }
    
    public class UpdateRadiologyLocationCommand : IRequest<ResultResponse<string>> 
    { 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public record UpdateRadiologyLogoCommand(IFormFile File) : IRequest<ResultResponse<string>>;

    public class AddRadiologyScanCommand : IRequest<ResultResponse<string>>
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Duration { get; set; } = null!;
        public string Preparation { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateRadiologyScanCommand : AddRadiologyScanCommand
    { 
        public string Id { get; set; } = null!; 
    }

    public record DeleteRadiologyScanCommand(string Id) : IRequest<ResultResponse<string>>;

    public record ChangeRadiologyAppointmentStatusCommand(string Id, Models.Enums.LabBookingStatusEnum Status) : IRequest<ResultResponse<string>>;

    public record UploadRadiologyReportCommand(string Id, IFormFile ReportFile, List<IFormFile>? Images, string? DoctorNotes) : IRequest<ResultResponse<string>>;
    public record DeleteRadiologyReportCommand(string Id) : IRequest<ResultResponse<string>>;

    public record MarkRadiologyNotificationReadCommand(string Id) : IRequest<ResultResponse<string>>;

    public class BookRadiologyScanCommand : IRequest<ResultResponse<string>>
    {
        public string RadiologyCenterId { get; set; } = null!;
        public string ScanId { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string ScheduledTimeSlot { get; set; } = null!;
    }

    public record CancelPatientRadiologyBookingCommand(string Id) : IRequest<ResultResponse<string>>;
}
