using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PharmacyProduct : ModelBase
    {
        // public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Category { get; set; }
        public string PharmacyId { get; set; } = default!;
        public string? Manufacturer { get; set; }
        public string? Barcode { get; set; }

        public Pharmacy Pharmacy { get; set; }
        // Navigation Property
        public ICollection<Inventory> Inventories { get; set; }
    }
}
