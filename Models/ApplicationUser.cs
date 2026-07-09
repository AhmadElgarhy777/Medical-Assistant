
using Microsoft.AspNetCore.Identity;
using Models.Enums;
using Models.Models;
using System.Collections.ObjectModel;

namespace Models
{
    public class ApplicationUser :IdentityUser
    {

        public string Address { get; set; } = null!;
        public Governorate Governorate { get; set; }
        public string City { get; set; } = null!;
        public string Role { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string? Img { get; set; }
     

        public ICollection<ConversationParticipant> conversationParticipants { get; set; } 
            = new List<ConversationParticipant>();


        public ICollection<Messages> Messages { get; set; } 
            = new List<Messages>();


        public ICollection<Notification> ReceivedNotifications { get; set; }
    = new HashSet<Notification>();

        public ICollection<Notification> SentNotifications { get; set; }
            = new HashSet<Notification>();

  
    }
}
