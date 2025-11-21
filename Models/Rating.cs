using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Rating
    {
        public string RatingId { get; set; } = null!;
        public StarsRatingEnum Stars { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

    }
}
