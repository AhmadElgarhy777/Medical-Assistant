using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class NearestDoctorsDto
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public double DoctorRating { get; set; }
    }
}