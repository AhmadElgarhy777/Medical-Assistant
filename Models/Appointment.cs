using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models
{
    public class Appointment
    {
        public string AppointmentId { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bookStatusEnum Status { get; set; }
        public BookTypeEnum Type { get; set; }
        public BookPaymentStatusEnum PaymentStatus { get; set; }
        public string PatientId { get; set; }

        public Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }


    }
}
