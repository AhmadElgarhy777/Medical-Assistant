using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PharmaciesDTO
    {

        public string ID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Governorate { get; set; }
        public string City { get; set; } = null!;
        public ConfrmationStatus Status { get; set; }
        public string? RealImg { get; set; }
        public string PharmacyLicense { get; set; } = null!;
        public string Gender { get; set; }
        public DateOnly BD { get; set; }
        public double RattingAverage { get; set; }
    }
}
