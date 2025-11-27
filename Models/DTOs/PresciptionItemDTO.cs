using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PresciptionItemDTO
    {
        public string DrugName { get; set; } = null!;
        public string Dos { get; set; } = null!;
        public string? Note { get; set; }
        public string? Duration { get; set; }
    }
}
