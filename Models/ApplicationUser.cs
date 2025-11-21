
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace Models
{
    public class ApplicationUser :IdentityUser
    {

        public string Address { get; set; } = null!;
        public string Role { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public Collection<ChatMessage> Messages { get; set; }
    }
}
