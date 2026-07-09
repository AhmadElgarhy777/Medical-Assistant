using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums.NotificationEnums
{
    public enum NotificationTypeEnum
    {
        AppointmentBooked,
        AppointmentAccepted,
        AppointmentRejected,
        AppointmentCancelled,

        ScanRequestedToPatient,
        ScanImagesForRequestUploaded,
        ScanRequestApproved,
        ScanRequestRejected,

        AIReportGeneratedSuccessfully,
        AiReportApproved,
        AIReportPublishedCompeletlly,


        ScanRequestCancelledByDoctor,







        //Lab

        LabCoolectorAssign,


    }
}
