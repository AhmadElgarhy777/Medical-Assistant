using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class LabSchedule : ModelBase
    {
        public string LabId { get; set; } = null!;
        [ForeignKey("LabId")]
        public Lab Lab { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }
        
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
