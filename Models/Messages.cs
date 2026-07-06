using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Messages : ModelBase
    {
        public string ConversationId { get; set; } = null!;
        public Conversation Conversation { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public ApplicationUser Sender { get; set; } = null!;
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }      //  رابط الصورة/الملف/الصوت
        public string? MediaType { get; set; }     //  Image/File/Voice
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
