using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DoctorsDTO
    {

        public string ID { get; set; } = null!;
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;
        public DateOnly BD { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string CrediateImg { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Bio { get; set; }
        public string Experence { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string CertificationImg { get; set; } = null!;

        public double RattingAverage { get; set; }
        public string SpecializationTitle { get; set; } = null!;
    }
}
