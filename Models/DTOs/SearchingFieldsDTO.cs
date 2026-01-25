using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.DTOs
{
    public class DocSearchingFieldsDTO
    {
        
        public string? Name { get; set; }
        [Required]
        public string? Specilzation { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public Governorate? Governorate { get; set; }
        public StarsRatingEnum? Rate { get; set; }
    }
}
