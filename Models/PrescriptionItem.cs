using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PrescriptionItem: ModelBase
    {
        public string DrugName { get; set; } = null!;
        public string Dos { get; set; } = null!;
        public string? Note { get; set; } 
        public string? Duration { get; set; }
        public string PresciptionId { get; set; }
        public Presciption Presciption { get; set; }


    }
}
