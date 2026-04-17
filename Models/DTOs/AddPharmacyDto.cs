using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddPharmacyDto
    {
        [Required(ErrorMessage = "ادخل اسم الصيدلية!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ادخل العنوان!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "ادخل التليفون!")]
        [Length(11, 11, ErrorMessage = "التليفون لازم يكون 11 رقم!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "التليفون لازم يكون أرقام بس!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "ادخل المحافظة!")]
        public string Governorate { get; set; }

        [Required(ErrorMessage = "ادخل المدينة!")]
        public string City { get; set; }

        public ConfrmationStatus Status { get; set; }
        public string? Gender { get; set; }
        public string? PharmacyLicense { get; set; }
        public string? RealImg { get; set; }
        public DateOnly BD { get; set; }
    }

    public class AddProductDto
    {
        [Required(ErrorMessage = "ادخل اسم الدواء!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ادخل وصف الدواء!")]
        public string Description { get; set; }

        public string? Category { get; set; }
    }

    public class AddInventoryDto
    {
       
        [Required(ErrorMessage = "ادخل ID الدواء!")]
        public string PharmacyProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "السعر لازم يكون أكبر من 0!")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "الكمية لازم تكون أكبر من 0!")]
        public int Quantity { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}