using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LabTestOffer : ModelBase
    {
        public string LabId { get; set; } = null!;
        public Lab Lab { get; set; } = null!;

        public string MedicalTestId { get; set; } = null!;
        public MedicalTest MedicalTest { get; set; } = null!;

        public decimal? Price { get; set; } // لو null بياخد BasePrice بتاع التحليل
        public bool IsAvailable { get; set; } = true;
    }
}
