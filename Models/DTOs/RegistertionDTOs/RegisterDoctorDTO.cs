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
    public class RegisterDoctorDTO
    {
        [Required(ErrorMessage ="The first Name is Required")]
        public string FName { get; set; } = null!;

        
        [Required(ErrorMessage ="The Middle Name is Required")]
        public string MName { get; set; } = null!;


        [Required(ErrorMessage = "The Last Name is Required")]
        public string LName { get; set; } = null!;


        [Required]
        [Length(14, 14, ErrorMessage = "The SSN Is Required 14 number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "SSN must contain numbers only.")]
        public string SSN { get; set; } = null!;


        [Required(ErrorMessage = "The User Name is Required")]
        public string UserName { get; set; } = null!;


        [Required(ErrorMessage = "The Email  is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;



        [Required(ErrorMessage = "The Gender  is Required")]
        [EnumDataType(typeof(GenderEnum),ErrorMessage ="The gender Is Invalid")]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "The Image  is Required")]
        public IFormFile Img { get; set; } = null!;


        [Required(ErrorMessage = "The Birht Date  is Required")]
        public DateOnly BirthDate { get; set; }


        [Required(ErrorMessage = "The Governorate is Required")]
        [EnumDataType(typeof(Governorate), ErrorMessage = "The Governorate Is Invalid")]
        public Governorate Governorate { get; set; }


        [Required(ErrorMessage = "The City is Required")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "The AddressDetails is Required")]
        public string AddressInDetails { get; set; } = null!;


        [Required(ErrorMessage = "The Phone number is Required")]
        [Length(11, 11, ErrorMessage = "The Phone Is Required 11 number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone must contain numbers only.")]
        public string Phone { get; set; } = null!;


        [Required(ErrorMessage = "The Experence is Required")]
        public string Experence { get; set; } = null!;


        [Required(ErrorMessage = "The Degree is Required")]
        public string Degree { get; set; } = null!;


      
        [Required(ErrorMessage = "The Image  is Required")]

        public IFormFile CertificationImg { get; set; } = null!;


        [Required(ErrorMessage = "The Image  is Required")]

        public IFormFile CrediateImg { get; set; } = null!;


        [Required(ErrorMessage = "The Specialization is Required")]
        public string SpecializationId { get; set; } = null!;

        [Required(ErrorMessage = "The Password Image is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        
        [Required(ErrorMessage = "The Confirmaion Password Image is Required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The Confirmation Of The Password Is Not Correct....! ")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
