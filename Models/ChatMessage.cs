using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ChatMessage: ModelBase
    {
        public string Test { get; set; } = null!;
        public string? Attachment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ChatId { get; set; }
        public Chat? Chat { get; set; }

        public ApplicationUser User { get; set; }
        public string? ApplicationUserId { get; set; }


    }
}
