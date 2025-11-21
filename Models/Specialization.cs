using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Specialization
    {
        public string SpecializationId { get; set; } = null!;
        public string Name { get; set; } = null!;
       
        public Collection<Doctor>? Doctor { get; set; }
    }
}
