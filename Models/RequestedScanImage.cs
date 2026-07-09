using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RequestedScanImage : ModelBase
    {
        public string ImagePath { get; set; } = null!;

        public string ScanRequestId { get; set; } = null!;

        public ScanRequest ScanRequest { get; set; } = null!;
    }
}
