using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class LowStockDto
    {
        public string InventoryId { get; set; }
        public string ProductName { get; set; }
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}