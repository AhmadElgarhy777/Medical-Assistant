using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DoctorStatsDTO
    {
        public int TotalToday { get; set; }      // إجمالي مواعيد النهاردة
        public int CompletedToday { get; set; }  // اللي كشفوا وخلصوا
        public int PendingToday { get; set; }    // اللي لسه في الانتظار
        public int CancelledToday { get; set; }  // اللي لغوا حجزهم
    }
}
