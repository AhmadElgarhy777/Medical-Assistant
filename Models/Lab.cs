using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Enums;

namespace Models
{
    public class Lab : ModelBase
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string AreaId { get; set; } = null!;
        public Area Area { get; set; } = null!;

        public string LabLicense { get; set; } = null!;
        public ConfrmationStatus Status { get; set; } = ConfrmationStatus.Pending;

        public string? ImageUrl { get; set; }
        public double Rating { get; set; } = 0;
        public int ReviewsCount { get; set; } = 0;
        public string WorkingHours { get; set; } = "9:00 AM - 10:00 PM";
        public bool SupportsHomeCollection { get; set; } = true;
        public bool IsActive { get; set; } = false; // بيبقى true بس بعد موافقة الأدمن

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public ICollection<LabTestOffer> TestOffers { get; set; } = new List<LabTestOffer>();
    }
}