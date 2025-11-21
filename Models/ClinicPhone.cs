using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClinicPhone
    {
        public string ClinicPhoneId { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}
