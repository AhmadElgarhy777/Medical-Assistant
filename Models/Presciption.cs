using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Presciption: ModelBase
    {
        public string? Diagnosis { get; set; }
        public DateTime CraetedAt { get; set; }
        public string PatientId { get; set; } = null!;
        public Patient? Patient { get; set; }
        public string DoctorId { get; set; } = null!;
        public Doctor? Doctor { get; set; }
        public Collection<PrescriptionItem> items { get; set; }
    }
}
