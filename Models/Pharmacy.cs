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
        public string Name { get; set; }=null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Governorate { get; set; }
        //public string Email { get; set; } = null!;
        public string City { get; set; } = null!;
        public ConfrmationStatus Status { get; set; }
        public string? RealImg { get; set; }
        public string PharmacyLicense { get; set; } = null!;
        public string Gender { get; set; }
        public DateOnly BD { get; set; }
        public double RattingAverage { get; set; }

       
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<PharmacyProduct> PharmacyProducts { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
