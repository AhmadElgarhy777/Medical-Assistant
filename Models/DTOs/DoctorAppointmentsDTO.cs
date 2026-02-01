using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DoctorAppointmentsDTO
    {
        public string ID { get; set; } = null!;
        public string PatientName { get; set; }
        public string AppointmentDate { get; set; }
        public string StartTime { get; set; }
        public string Status { get; set; } // Pending, Confirmed, etc.
        public string BookingType { get; set; } // Online or Clinic
    }
}
