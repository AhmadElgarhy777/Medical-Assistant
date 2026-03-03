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

        // Navigation Property
        public ICollection<Inventory> Inventories { get; set; }
    }
}
