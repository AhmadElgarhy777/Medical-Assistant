using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Models.Enums;

namespace Models
{
    public class Area : ModelBase
    {
        public string Name { get; set; } = null!;      // مدينة نصر، المعادي...
        public Governorate Governorate { get; set; }    
    }
}