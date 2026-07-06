using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PrescriptionItemDto
    {
        public string InventoryId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}