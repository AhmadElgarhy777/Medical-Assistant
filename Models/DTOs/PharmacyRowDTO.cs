using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PharmacyRowDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Governorate { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public ConfrmationStatus Status { get; set; }
        public string? RealImg { get; set; }
    }
}
