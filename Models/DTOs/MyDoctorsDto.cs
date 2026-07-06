using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class MyDoctorsDto
    {
        public string DoctorID { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public string DoctorSpecilization { get; set; } = null!;
        public string DoctorImg { get; set; } = null!;

        public int AppoinmentCount
        {
            get; set;


        }
    }
}
