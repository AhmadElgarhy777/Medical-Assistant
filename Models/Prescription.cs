using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Prescription : ModelBase
    {
        public string? Diagnosis { get; set; }
        public DateTime CraetedAt { get; set; } // سيبتها زي ما هي عندك عشان متضربش
        public string PatientId { get; set; } = null!;
        public Patient? Patient { get; set; }
        public string DoctorId { get; set; } = null!;
        public Doctor? Doctor { get; set; }
        public Collection<PrescriptionItem> items { get; set; } = new Collection<PrescriptionItem>();
        public string? AppointmentId { get; set; }
    }
}
