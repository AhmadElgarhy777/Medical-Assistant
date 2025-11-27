using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DocSearchingFieldsDTO
    {
        public string? Name { get; set; }
        public string? Specilzation { get; set; }
        public string? City { get; set; }
        public Governorate? Governorate { get; set; }
        public StarsRatingEnum? Rate { get; set; }
    }
}
