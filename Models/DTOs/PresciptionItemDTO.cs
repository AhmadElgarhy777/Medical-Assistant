using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PresciptionItemDTO
    {
        public string? InventoryId { get; set; }
        public string? MedicineName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
