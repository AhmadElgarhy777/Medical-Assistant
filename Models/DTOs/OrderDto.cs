using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateOrderDto
    {
        public string PatientId { get; set; }
        public string PharmacyId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public string InventoryId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderResultDto
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string InvoiceId { get; set; }
        public decimal InvoiceTotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}