using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DoctorAvailableTimeDTO
    {
        public string Id { get; set; } // مهم عشان لما يختار ميعاد نبعت الـ ID ده
        public string Day { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
