using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Patient
    {
        public string PatientId { get; set; } = null!;
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public GenderEnum Gender { get; set; } 
        public string Img { get; set; } = null!;
        public DateOnly BD { get; set; }
        public string Email { get; set; } = null!;

        public Governorate Governorate { get; set; }

        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;

        public Collection<PatientPhone> patientPhones { get; set; } = null!;
        public Collection<Appointment>? appointments { get; set; }
        public Collection<AiReport>? AiReports { get; set; }
        public Collection<Rating>? Ratings { get; set; }
        public Collection<Presciption>? presciptions { get; set; }
        public Collection<Chat>? Chats { get; set; }
        public Collection<DoctorPatient>? DoctorPatients { get; set; }

    }
}
