using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.RegistertionDTOs
{
    public class RegisterLabDTO
    {
        [Required(ErrorMessage = "اسم المعمل مطلوب")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "الإيميل مطلوب")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رقم التليفون مطلوب")]
        [Length(11, 11, ErrorMessage = "رقم التليفون لازم يكون 11 رقم")]
        [RegularExpression(@"^\d+$", ErrorMessage = "رقم التليفون أرقام بس")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "لازم تحدد الحي")]
        public string AreaId { get; set; } = null!;

        [Required(ErrorMessage = "ترخيص المعمل مطلوب")]
        public string LabLicense { get; set; } = null!;

        public IFormFile? Img { get; set; }

        public bool SupportsHomeCollection { get; set; } = true;

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "تأكيد كلمة السر مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "تأكيد كلمة السر مش مطابق")]
        public string ConfirmPassword { get; set; } = null!;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}