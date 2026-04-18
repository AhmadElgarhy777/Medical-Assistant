using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ShowCommentDTO
    {
        public string PatientId { get; set; } = default!;
        public string PatientName { get; set; } = default!;
        public string Comment { get; set; } = default!;
        public DateTime CreatedAt { get; set; }


    }
}
