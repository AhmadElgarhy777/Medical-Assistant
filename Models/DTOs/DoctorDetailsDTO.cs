using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DoctorDetailsDTO
    {
        public string ID { get; set; } = null!;
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Img { get; set; } = null!;
        public DateOnly BD { get; set; }
        public int Age { get; set; }
        public string Governorate { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string CrediateImg { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string CertificationImg { get; set; } = null!;
        public string Specialization { get; set; } = null!;

    }
}
