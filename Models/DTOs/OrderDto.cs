using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "ادخل ID المريض!")]
        public string PatientId { get; set; }

        [Required(ErrorMessage = "ادخل ID الصيدلية!")]
        public string PharmacyId { get; set; }

        [Required(ErrorMessage = "لازم يكون في أدوية في الطلب!")]
        [MinLength(1, ErrorMessage = "لازم يكون في دواء واحد على الأقل!")]
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        [Required(ErrorMessage = "ادخل ID الدواء!")]
        public string InventoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "الكمية لازم تكون أكبر من 0!")]
        public int Quantity { get; set; }
    }

    public class OrderResultDto
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string? InvoiceId { get; set; }
        public decimal InvoiceTotalAmount { get; set; }
        public string? PaymentStatus { get; set; }
    }
}