using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SuperAdminDashboardDto
    {
        public int NewUsersToday { get; set; }
        public int PendingPharmacies { get; set; }
        public int PendingDoctors { get; set; }
        public int TotalOrdersToday { get; set; }
        public decimal TotalSalesToday { get; set; }
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPharmacies { get; set; }
    }
}