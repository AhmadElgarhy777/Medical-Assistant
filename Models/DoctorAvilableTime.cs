using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DoctorAvilableTime
    {
        public string DoctorAvilableTimeId { get; set; } = null!;
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
