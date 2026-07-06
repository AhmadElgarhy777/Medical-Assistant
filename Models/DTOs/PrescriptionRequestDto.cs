using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PrescriptionRequestDto
    {
        public string PatientId { get; set; } = null!;
        public string PharmacyId { get; set; } = null!;
        public IFormFile PrescriptionImg { get; set; } = null!;
        public string? Notes { get; set; }
        public string? PharmacyNotes { get; set; }

    }
}
