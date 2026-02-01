using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.DTOs
{
    public class DoctorDTO
    {
        public string ID { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Img { get; set; } = null!;
        public int Age { get; set; } 
        public string Governorate { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double RattingAverage { get; set; }
        public string Bio { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public List<string> ClincNumbers { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public string Degree { get; set; } = null!;
    }
}
