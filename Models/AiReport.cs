using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AiReport
    {
        public string AiReportId { get; set; } = null!;
        public string AiReportOutput { get; set; } = null!;
        public string Img { get; set; } = null!;
        public string DoctorNote { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }




    }
}
