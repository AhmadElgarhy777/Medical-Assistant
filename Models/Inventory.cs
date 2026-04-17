using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Inventory : ModelBase
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        // ✅ غيرناهم string عشان الـ ID في ModelBase string
        // public string PharmacyId { get; set; }
        // public string PharmacyProductId { get; set; }
        public string PharmacyId { get; set; }
        public string PharmacyProductId { get; set; }
        // Navigation Properties
        public Pharmacy Pharmacy { get; set; }
        public PharmacyProduct PharmacyProduct { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}