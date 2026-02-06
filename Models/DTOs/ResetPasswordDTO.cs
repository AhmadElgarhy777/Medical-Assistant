using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
