using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Enums;

namespace Models
{
    public class LabTestResult : ModelBase
    {
        public string LabBookingItemId { get; set; } = null!;
        public LabBookingItem LabBookingItem { get; set; } = null!;

        public ResultStatusEnum Status { get; set; } = ResultStatusEnum.NotReady;
        public string? ResultFileUrl { get; set; }
        public string? ResultValuesJson { get; set; }
        public string? DoctorNotes { get; set; }
        public DateTime? ReportedAt { get; set; }
    }
}
