using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class BookingRequestDTO
    {
        public string PatientId { get; set; } = null!;
        public string NurseId { get; set; } = null!;

    }
}
