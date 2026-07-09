using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SendTextRequestDto
    {
        public string Text { get; set; } = null!;
    }

    public class ConversationDto
    {
        public string ConversationId { get; set; } = null!;
        public string OtherPartyName { get; set; } = null!;
        public string? OtherPartyImage { get; set; }
        public string OtherPartyType { get; set; } = null!; // Doctor, Patient, Lab, Radiology
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
    }

    public class MessageDto
    {
        public string MessageId { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }
        public string? MediaType { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }
    }
}
