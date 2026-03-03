using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Conversation:ModelBase
    {
        public ConversationType Type { get; set; }

        public ICollection<ConversationParticipant> conversationParticipants { get; set; } 
            = new List<ConversationParticipant>();

        public ICollection<Messages> Messages { get; set; }
            = new List<Messages>();



    }
}
