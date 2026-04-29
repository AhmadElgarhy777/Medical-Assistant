using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PharmcyProductDTO
    {
        public string PharmcyProductId { get; set; } = default!;
        public string Name { get; set; }= default!;
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
