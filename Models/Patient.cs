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

        public Collection<PatientPhone> patientPhones { get; set; } = null!;
        public Collection<Appointment>? appointments { get; set; }
        public Collection<AiReport>? AiReports { get; set; }
        public Collection<Rating>? Ratings { get; set; }
        public Collection<Presciption>? presciptions { get; set; }
        public Collection<Chat>? Chats { get; set; }
        public Collection<DoctorPatient>? DoctorPatients { get; set; }

    }
}
