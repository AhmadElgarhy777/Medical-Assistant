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
        Cancelled = 6,

        // Lab Specific Statuses
        Pending = 7,
        Accepted = 8,
        CollectorAssigned = 9,
        Processing = 10,
        Rejected = 11,
        CollectorStartedTrip = 16,
        ReturnedToLab = 17,

        // Radiology Specific Statuses
        PatientArrived = 12,
        Scanning = 13,
        DoctorReviewing = 14,
        ReportReady = 15
    }

    public enum ResultStatusEnum { NotReady = 1, Ready = 2, ReviewedByDoctor = 3 }
}