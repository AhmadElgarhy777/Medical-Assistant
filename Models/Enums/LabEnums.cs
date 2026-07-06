using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum ServiceTypeEnum { Lab = 1, Radiology = 2 }

    public enum VisitTypeEnum { AtCenter = 1, HomeCollection = 2 }

    public enum LabBookingStatusEnum
    {
        PendingPayment = 1,
        Confirmed = 2,
        SampleCollected = 3,
        InProgress = 4,
        Completed = 5,
        Cancelled = 6
    }

    public enum ResultStatusEnum { NotReady = 1, Ready = 2, ReviewedByDoctor = 3 }
}