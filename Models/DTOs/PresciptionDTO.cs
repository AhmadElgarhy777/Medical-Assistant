using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PresciptionDTO
    {
        public string ID { get; set; } = null!;
        public string? Diagnosis { get; set; }
        public DateTime CraetedAt { get; set; }
        public string DoctorName { get; set; } = null!;
    }
}
