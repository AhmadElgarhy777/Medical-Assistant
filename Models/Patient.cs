using Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Patient: ModelBase
    {
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string? Img { get; set; }
        public DateOnly BD { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? BloodType { get; set; }


        public ICollection<PatientPhone> patientPhones { get; set; } = null!;
        public ICollection<Appointment>? appointments { get; set; }
        public ICollection<AiReport>? AiReports { get; set; }=new List<AiReport>(); 
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<DoctorPatient>? DoctorPatients { get; set; }

    }
}
