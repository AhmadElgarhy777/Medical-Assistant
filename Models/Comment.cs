using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Comment:ModelBase
    {
        public string CommentText { get; set; }=default!;       
        public DateTime CreatedAt { get; set; }
        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string? TargetId { get; set; }
        public string? TargetRole { get; set; }
        public ApplicationUser? Target { get; set; }
    }
}
