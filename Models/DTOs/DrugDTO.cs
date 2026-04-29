using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DrugDTO
    {
        public string Name { get; set; }= default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Category { get; set; }
        public string InvetoryId { get; set; } = default!;
    }
}
