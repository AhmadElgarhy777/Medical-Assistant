using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.DTOs;

namespace Features.PharmacyFeature
{
    public interface IOrderService
    {
        Task<OrderResultDto> CreateOrderAsync(CreateOrderDto dto);
        Task<IEnumerable<OrderResultDto>> GetPatientOrdersAsync(string patientId);
        Task<OrderResultDto> UpdateOrderStatusAsync(string orderId, string status);
        Task<IEnumerable<OrderResultDto>> GetPharmacyOrdersAsync(string pharmacyId);
        Task<OrderResultDto> UpdateInvoicePaymentAsync(string orderId, string paymentStatus, string paymentMethod);
        Task<OrderResultDto> CancelOrderAsync(string orderId);
    }
}