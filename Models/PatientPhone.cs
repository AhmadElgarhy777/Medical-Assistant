using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PatientPhone
    {
        public string PatientPhoneId { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
