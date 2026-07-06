using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    // نوع التحليل نفسه: CBC، سكر صايم... (بدون سعر خاص بمعمل معين)
    public class MedicalTest : ModelBase
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // دم، سكر، كبد...
        public decimal BasePrice { get; set; }
        public int TurnaroundHours { get; set; } = 24;
        public string PreparationInstructions { get; set; } = string.Empty;
        public bool RequiresFasting { get; set; } = false;

        public ICollection<LabTestOffer> LabOffers { get; set; } = new List<LabTestOffer>();
    }
}