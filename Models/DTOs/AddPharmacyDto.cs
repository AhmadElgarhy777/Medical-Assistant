using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddPharmacyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Status { get; set; } = "Active";
        public string? Gender { get; set; } // ✅ هنا صح
        public string? PharmacyLicense { get; set; } // ✅ زود ده
        public string? RealImg { get; set; } // ✅ وده عشان ميجيش نفس المشكلة
        public DateOnly BD { get; set; } 
    }

    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Category { get; set; }
    }

    public class AddInventoryDto
    {
        public string PharmacyId { get; set; }
        public string PharmacyProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}