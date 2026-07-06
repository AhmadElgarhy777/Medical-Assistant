using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BanReport : ModelBase
    {
        public string BannedUserId { get; set; } = null!;
        public string BannedUserName { get; set; } = null!;
        public string BannedUserEmail { get; set; } = null!;
        public string AdminId { get; set; } = null!;
        public string AdminName { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string UserType { get; set; } = null!; // Doctor, Nurse, Pharmacy, Patient
        public DateTime BanDate { get; set; } = DateTime.Now;
        public int BanCount { get; set; } = 1;
    }
}
