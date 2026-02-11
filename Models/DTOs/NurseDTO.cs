using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.DTOs
{
    public class NurseDTO
    {
        public string ID { get; set; } = null!;  
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;


        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;

        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string? Certification { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public double RattingAverage { get; set; }
        public string Bio { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public decimal PricePerDay { get; set; }


    }
}
