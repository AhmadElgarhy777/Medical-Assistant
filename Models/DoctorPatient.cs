using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DoctorPatient
    {

        public string DoctorPatientId { get; set; } = null!;
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public string? PatientId { get; set; } 
        public string? DoctorId { get; set; } 



    }
}
