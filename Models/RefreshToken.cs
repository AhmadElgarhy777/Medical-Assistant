using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RefreshToken:ModelBase
    {
        public string TokenHash { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Expired { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public string? ReplacedByTokenHash { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceInfo { get; set; }
        public ApplicationUser User { get; set; }

    }
}
