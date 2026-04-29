using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class NearestPharmacyDto
    {
        public string PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Distance { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
