using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PharmacyDashboardDto
    {
        public int PendingOrders { get; set; }
        public int LowStockCount { get; set; }
        public decimal TodaySales { get; set; }
        public int TotalInventory { get; set; }
        public double AverageRating { get; set; }
    }
}