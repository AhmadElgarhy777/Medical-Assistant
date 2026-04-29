using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Admin:ModelBase
    {
        public string UserName { get; set; } = null!;
         public string SSN { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public Governorate Governorate { get; set; }
        public string City { get; set; } = null!;

    }
}
