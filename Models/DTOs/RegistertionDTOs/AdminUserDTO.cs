using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AdminUserDTO
    {
        [Required(ErrorMessage = "This First Name Is Required")]
        public string FName { get; set; } = null!;
        [Required(ErrorMessage ="This Last Name Is Required")]
        public string LName { get; set; } = null!;
        [Required]
        [Length(14, 14,ErrorMessage ="The SSN Is Required 14 number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "SSN must contain numbers only.")]
        public string SSN { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This Email Is Required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "This Address Is Required")]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "This Gender Is Required")]
        [EnumDataType(typeof(GenderEnum), ErrorMessage = "Invalid Gender")]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "The Governorate is Required")]
        [EnumDataType(typeof(Governorate), ErrorMessage = "The Governorate Is Invalid")]
        public Governorate Governorate { get; set; }


        [Required(ErrorMessage = "The City is Required")]
        public string City { get; set; } = null!;


    }
}
