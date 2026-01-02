using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.DTOs
{
    public class AppointmentDTO
    {
        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bookStatusEnum Status { get; set; }
        public BookTypeEnum Type { get; set; }
        public BookPaymentStatusEnum PaymentStatus { get; set; }
        public string DoctorName { get; set; } = null!;

    }
}
