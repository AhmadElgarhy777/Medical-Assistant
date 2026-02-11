using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class BookingDTO
    {
        public string  BookingID { get; set; }  = null!;
        public string PatientId { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public int Age { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string PatientEmail{ get; set; } = null!;

    }
}
