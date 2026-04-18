using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PharmacyProfileDTO
    {
        public string ID { get; set; }=default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; }=default!;
        public string Phone { get; set; }=  default!;
        public string Governorate { get; set; }= default!;
        public string City { get; set; }=   default!;
        public string? RealImg { get; set; }
        public string PharmacyLicense { get; set; }=default!;
        public string? Gender { get; set; }
        public DateOnly BD { get; set; }
        public double RattingAverage { get; set; }

    }
}
