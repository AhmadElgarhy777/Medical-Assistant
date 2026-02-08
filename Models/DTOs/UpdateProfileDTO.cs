using Microsoft.AspNetCore.Http;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateProfileDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public Governorate Governorate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; } 
        public IFormFile? Img { get; set; }

    }
}
