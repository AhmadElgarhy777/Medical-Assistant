using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace DataAccess.Repositry.IRepositry
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<Inventory> GetInventoryByIdAsync(string inventoryId);
        Task<IEnumerable<Order>> GetPatientOrdersAsync(string patientId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetPharmacyOrdersAsync(string pharmacyId);
        Task<Invoice> GetInvoiceByOrderIdAsync(string orderId);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task UpdateInventoryAsync(Inventory inventory);
        Task CancelOrderAsync(Order order);

        Task<IEnumerable<Order>> GetPharmacyOrdersByStatusAsync(string pharmacyId, string status);
    }
}