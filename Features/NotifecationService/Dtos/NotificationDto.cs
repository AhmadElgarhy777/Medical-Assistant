using Models.Enums.NotificationEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NotifecationService.Dtos
{
    public class NotificationDto
    {
        public string ID { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public NotificationTypeEnum Type { get; set; }

        public NotificationReferenceType ReferenceType { get; set; }

        public string? ReferenceId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
