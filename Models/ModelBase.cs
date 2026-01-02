using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models
{
    public class ModelBase
    {
        public string ID { get; set; } 
        public string SSN { get; set; } 
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public GenderEnum Gender { get; set; }
        public string Img { get; set; } 
        public DateOnly BD { get; set; }
        public Governorate Governorate { get; set; }
        public string Address { get; set; } 
        public string City { get; set; } 

    }
}
