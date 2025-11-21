using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Nures
    {
        public string NuresId { get; set; } = null!;
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;
        public string CrediateImg { get; set; } = null!;

        public DateOnly BD { get; set; }
        public string Email { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string? Certification { get; set; }
        public string OrgnizationNumber { get; set; } = null!;
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string RattingAverage { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public string PricePerDay { get; set; } = null!;

        public ConfrmationStatus Status { get; set; } = ConfrmationStatus.Pending;

    }
}
