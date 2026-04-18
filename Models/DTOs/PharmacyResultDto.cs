using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;
using Models;

namespace Models.DTOs
{
    public class PharmacyResultDto
    {
        public string PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Phone { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public double RattingAverage { get; set; }
    }
}
