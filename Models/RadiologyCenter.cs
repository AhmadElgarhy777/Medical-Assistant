using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RadiologyCenter : ModelBase
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string AreaId { get; set; } = null!;
        public Area Area { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public double Rating { get; set; } = 0;
        public int ReviewsCount { get; set; } = 0;
        public string WorkingHours { get; set; } = "9:00 AM - 10:00 PM";
        public bool IsActive { get; set; } = true;

        public ICollection<RadiologyCenterScan> ScanOffers { get; set; } = new List<RadiologyCenterScan>();
    }
}
