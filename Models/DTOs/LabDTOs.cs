using System;
using System.Collections.Generic;

namespace Models.DTOs
{
    public class LabDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string AreaId { get; set; } = null!;
        public string LabLicense { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string WorkingHours { get; set; } = null!;
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public bool IsActive { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class LabTestDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public string EstimatedTime { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsAvailable { get; set; }
    }

    public class LabBookingDto
    {
        public string Id { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string PatientPhone { get; set; } = null!;
        public string VisitType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string ScheduledTimeSlot { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public string? HomeAddress { get; set; }
        public List<LabBookingItemDto> Items { get; set; } = new List<LabBookingItemDto>();
    }

    public class LabBookingItemDto
    {
        public string TestName { get; set; } = null!;
        public decimal Price { get; set; }
        public string ResultStatus { get; set; } = null!;
        public string? ResultFileUrl { get; set; }
    }

    public class LabDashboardDto
    {
        public int TodaysBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CompletedBookings { get; set; }
        public decimal Revenue { get; set; }
        public int HomeCollectionCount { get; set; }
        public List<LabBookingDto> RecentActivity { get; set; } = new List<LabBookingDto>();
    }
}
