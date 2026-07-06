using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record LabBookingSummaryDTO(
        string Id,
        string ServiceType,
        string VisitType,
        string ProviderName,
        DateTime ScheduledDate,
        string ScheduledTimeSlot,
        string Status,
        decimal TotalPrice,
        int ItemsCount);
}