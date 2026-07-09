using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Enums;

namespace Models
{
    // اسمها LabBooking مش Booking عشان الاسم مأخوذ بالفعل (حجز الممرضة)
    public class LabBooking : ModelBase
    {
        public string PatientId { get; set; } = null!; // بياخده من التوكن، مش من الـ Request
        public Patient? Patient { get; set; }


        public ServiceTypeEnum ServiceType { get; set; }
        public VisitTypeEnum VisitType { get; set; }

        public string? LabId { get; set; }
        public Lab? Lab { get; set; }

        public string? RadiologyCenterId { get; set; }
        public RadiologyCenter? RadiologyCenter { get; set; }

        public string? AreaId { get; set; }
        public Area? Area { get; set; }
        public string? HomeAddress { get; set; }

        public DateTime ScheduledDate { get; set; }
        public string ScheduledTimeSlot { get; set; } = string.Empty;

        public LabBookingStatusEnum Status { get; set; } = LabBookingStatusEnum.PendingPayment;

        public decimal SubTotal { get; set; }
        public decimal HomeCollectionFee { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; } = "Cash";
        public bool IsPaid { get; set; } = false;
        public string? Notes { get; set; }
        public string? CollectorId { get; set; }

        public ICollection<LabBookingItem> Items { get; set; } = new List<LabBookingItem>();
    }
}