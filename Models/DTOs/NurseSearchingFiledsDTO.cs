using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class NurseSearchingFiledsDTO
    {
        public string? Name { get; set; }
        public string? City { get; set; }
        public Governorate? Governorate { get; set; }
        public StarsRatingEnum? Rate { get; set; }
        public GenderEnum? Gender { get; set; }
        public decimal? MinPrice { get; set; }  
        public decimal? MaxPrice { get; set; }

    }
}
