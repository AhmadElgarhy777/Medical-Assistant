using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Models
{
    public class PrescriptionRequest : ModelBase
    {
        public string PatientId { get; set; } = null!;
        public string PharmacyId { get; set; } = null!;
        public IFormFile PrescriptionImg { get; set; } = null!;
        public string? Notes { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? PharmacyNotes { get; set; }
        public string? OrderId { get; set; } // ✅ جديد
        public Pharmacy Pharmacy { get; set; } = null!;
    }
}