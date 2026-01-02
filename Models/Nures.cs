
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
        public string CrediateImg { get; set; } = null!;
        public string Phone { get; set; }=null!;
        public string Degree { get; set; } = null!;
        public string? Certification { get; set; }
        public string OrgnizationNumber { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public decimal? PricePerDay { get; set; } = null!;
        public ConfrmationStatus Status { get; set; } = ConfrmationStatus.Pending;
        public Collection<Rating>? Ratings { get; set; }
        public Collection<Chat>? Chats { get; set; }

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
    }
}
