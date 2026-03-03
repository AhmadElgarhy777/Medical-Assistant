using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Messages:ModelBase
    {
        public string ConversationId { get; set; } = null!;
        public Conversation Conversation { get; set; } = null!;

        public string SenderId { get; set; } = null!;
        public ApplicationUser Sender { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime SentAt { get; set; }
    }
}
