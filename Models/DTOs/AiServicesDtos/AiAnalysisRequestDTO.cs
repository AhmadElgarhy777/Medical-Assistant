using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.AiServicesDtos
{
    public class AiAnalysisRequestDTO
    {
        [Required]
        public List<IFormFile> Images { get; set; } = [];

        [Required]
        public string PatientId { get; set; } = null!;

        [Required]
        public string DoctorId { get; set; } = null!;

        public string? DoctorNote { get; set; }
    }
}
