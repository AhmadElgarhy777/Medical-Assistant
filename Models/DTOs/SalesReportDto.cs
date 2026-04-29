using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SalesReportDto
    {
        public decimal DailySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal MonthlySales { get; set; }
        public IEnumerable<TopDrugDto> TopDrugs { get; set; }
        public IEnumerable<PeakHoursDto> PeakHours { get; set; }
    }
}