using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    namespace Models
    {
        public class Notification : ModelBase
        {
            public string ReceiverId { get; set; } = null!;
            public ApplicationUser Receiver { get; set; } = null!;

            public string? SenderId { get; set; }
            public ApplicationUser? Sender { get; set; }

            public string Title { get; set; } = null!;

            public string Body { get; set; } = null!;

            public NotificationTypeEnum Type { get; set; }

            public NotificationReferenceType ReferenceType { get; set; }

            public string? ReferenceId { get; set; }

            public bool IsRead { get; set; } = false;

            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }
    }
}
