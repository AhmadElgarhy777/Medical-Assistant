using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Pharmacy : ModelBase
    {
      //  public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public string RealImg { get; set; }
        public string PharmacyLicense { get; set; }
        public string? Gender { get; set; }
        public DateOnly BD { get; set; }

        // Navigation Property
        public ICollection<Inventory> Inventories { get; set; }
    }
}
