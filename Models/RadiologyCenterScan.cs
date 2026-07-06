using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RadiologyCenterScan : ModelBase
    {
        public string RadiologyCenterId { get; set; } = null!;
        public RadiologyCenter RadiologyCenter { get; set; } = null!;

        public string RadiologyScanId { get; set; } = null!;
        public RadiologyScan RadiologyScan { get; set; } = null!;

        public decimal? Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}