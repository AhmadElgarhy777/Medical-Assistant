using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Invoice : ModelBase
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }

        // Foreign Key
        public string OrderId { get; set; }

        // Navigation Property
        public Order Order { get; set; }
    }
}
