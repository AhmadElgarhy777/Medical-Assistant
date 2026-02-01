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
        public string Email { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string? Img { get; set; }
        public DateOnly BD { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? BloodType { get; set; }


        public Collection<PatientPhone> patientPhones { get; set; } = null!;
        public Collection<Appointment>? appointments { get; set; }
        public Collection<AiReport>? AiReports { get; set; }
        public Collection<Rating>? Ratings { get; set; }
        public Collection<Prescription>? Prescriptions { get; set; }
        public Collection<Chat>? Chats { get; set; }
        public Collection<DoctorPatient>? DoctorPatients { get; set; }

    }
}
