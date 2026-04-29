using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SuperAdminStatsDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPharmacies { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
    }
}