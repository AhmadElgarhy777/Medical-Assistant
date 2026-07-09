using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums.NotificationEnums
{
    public enum NotificationReferenceType
    {
        None,

        Appointment,

        ScanRequest,

        AiReport,

        Prescription,

        LabResult,

        Chat,

        MedicineOrder
    }
}
