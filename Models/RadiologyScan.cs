using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RadiologyScan : ModelBase
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // MRI, CT, X-Ray, Ultrasound
        public decimal BasePrice { get; set; }
        public int TurnaroundHours { get; set; } = 24;
        public string PreparationInstructions { get; set; } = string.Empty;
        public bool RequiresContrast { get; set; } = false;

        public ICollection<RadiologyCenterScan> CenterOffers { get; set; } = new List<RadiologyCenterScan>();
    }
}
