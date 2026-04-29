using Microsoft.AspNetCore.Http;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.RegistertionDTOs
{
    public class RegisterPharmacyDTO
    {
        [Required(ErrorMessage = "The Pharmacy Name is Required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "The Phone Number is Required")]
        [Length(11, 11, ErrorMessage = "The Phone Is Required 11 number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone must contain numbers only.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "The Address is Required")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "The City is Required")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "The Governorate is Required")]
        [EnumDataType(typeof(Governorate))]
        public Governorate Governorate { get; set; }

        [Required(ErrorMessage = "The Gender is Required")]
        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "The Birth Date is Required")]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "The Pharmacy License is Required")]
        public string PharmacyLicense { get; set; } = null!;

        public IFormFile? Img { get; set; }

        [Required(ErrorMessage = "The Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "The Confirmation Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Confirmation Of The Password Is Not Correct!")]
        public string ConfirmPassword { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}