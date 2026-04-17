using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PharmacyRating
    {
        public int Id { get; set; }
        public string PharmacyId { get; set; }
        public string PatientId { get; set; }
        public int Rating { get; set; } // من 1 لـ 5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public Pharmacy Pharmacy { get; set; }
    }
}
