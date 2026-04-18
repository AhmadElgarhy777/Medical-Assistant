using Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ConfrmationStatus Status { get; set; }
        public string? RealImg { get; set; }
        public string PharmacyLicense { get; set; }
        public string? Gender { get; set; }
        public DateOnly BD { get; set; }
        public double RattingAverage { get; set; }

        // public string Statuss { get; set; } = "Pending";

        // Navigation Property
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<PharmacyProduct> PharmacyProducts { get; set; }

    }
}
