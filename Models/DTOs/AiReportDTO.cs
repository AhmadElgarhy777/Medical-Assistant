using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AiReportDTO
    {
        public string AiReportOutput { get; set; } = null!;
        public string? Img { get; set; } 
        public string DoctorNote { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string DoctorName { get; set; }=null!;
        public string? DoctorSpeclization { get; set; }
    }
}
