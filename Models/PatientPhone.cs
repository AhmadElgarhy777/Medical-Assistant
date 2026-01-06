using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PatientPhone: ModelBase
    {
        public string Phone { get; set; } = null!;
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
