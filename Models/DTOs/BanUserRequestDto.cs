using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class BanUserRequestDto
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public string Reason { get; set; } = null!;
    }

    public class UnbanUserRequestDto
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
