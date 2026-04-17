using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class MedicineInvetoryListDTO
    {
        public string MedicineName { get; set; }=default!;
        public string? MedicineCategory { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        public string InvetoryId { get; set; }=default!;


    }
}
