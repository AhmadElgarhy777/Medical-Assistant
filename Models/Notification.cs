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
            public string UserId { get; set; } = null!; // تأكد من الحروف Capital
            public string Message { get; set; } = null!;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public bool IsRead { get; set; } = false;
        }
    }
}
