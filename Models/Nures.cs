
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models
{
    public class Nures: ModelBase
    {
        public string SSN { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;
        public DateOnly BD { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string CrediateImg { get; set; } = null!;
        public string Phone { get; set; }=null!;
        public string Degree { get; set; } = null!;
        public string CertificationImg { get; set; } = null!;
        public string? Bio { get; set; } 
        public string Experence { get; set; } = null!;
        public decimal PricePerDay { get; set; }
        public ConfrmationStatus Status { get; set; } = ConfrmationStatus.Pending;

        public double RattingAverage
        {
            get
            {
                if (Ratings == null || Ratings.Count == 0)
                    return 0;

                return Ratings.Average(r => (int)r.Stars);
            }
            set { }

        }
        public Collection<Rating>? Ratings { get; set; }
        public Collection<Chat>? Chats { get; set; }

    }
}
