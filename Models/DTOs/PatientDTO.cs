using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Models.DTOs
{
    public class PatientDTO
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!; // هنبعته كـ string للسهولة
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? BloodType { get; set; }

    }
}