using System;
using System.Collections.Generic;

namespace Models.DTOs
{
    public class RadiologyCenterDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string AreaId { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string WorkingHours { get; set; } = null!;
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = null!;
    }

    public class RadiologyScanOfferDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Duration { get; set; } = null!;
        public string Preparation { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class RadiologyAppointmentDto
    {
        public string Id { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string PatientPhone { get; set; } = null!;
        public string ScanName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string ScheduledTimeSlot { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public string? ResultFileUrl { get; set; }
        public string? ImagesUrls { get; set; }
        public string? DoctorNotes { get; set; }
    }

    public class RadiologyDashboardDto
    {
        public int TodaysAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public decimal Revenue { get; set; }
        public List<RadiologyAppointmentDto> RecentActivity { get; set; } = new List<RadiologyAppointmentDto>();
    }
}
