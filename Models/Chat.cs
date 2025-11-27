using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Chat
    {
        public string ChatId { get; set; } = null!;
        public DateTime CratedAt { get; set; }
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Nures Nures { get; set; }
        public string? NuresId { get; set; } 

        public Collection<ChatMessage> Messages { get; set; }
    }
}
