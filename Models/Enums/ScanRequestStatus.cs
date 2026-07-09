using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum ScanRequestStatus
    {
        Pending,

        Uploaded,

        Approved,

        Rejected,

        Analyzed,

        Completed,

        Cancelled,

        Expired
    }
}
