using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using Models;
using Models.DTOs;

namespace Features.PharmacyFeature
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResultDto> CreateOrderAsync(CreateOrderDto dto)
        {
            decimal totalAmount = 0;
            foreach (var item in dto.Items)
            {
                var inventory = await _orderRepository.GetInventoryByIdAsync(item.InventoryId);
                totalAmount += inventory.Price * item.Quantity;
            }

            var order = new Order
            {
                PatientId = dto.PatientId,
                PharmacyId = dto.PharmacyId,
                Date = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount
            };
            await _orderRepository.CreateOrderAsync(order);

            // ✅ إضافة الـ OrderItems مع تقليل الكمية
            foreach (var item in dto.Items)
            {
                var inventory = await _orderRepository.GetInventoryByIdAsync(item.InventoryId);

                // ✅ التحقق من الكمية
                if (inventory.Quantity < item.Quantity)
                    throw new Exception("الكمية مش كفاية!");

                var orderItem = new OrderItem
                {
                    OrderId = order.ID,
                    InventoryId = item.InventoryId,
                    Quantity = item.Quantity,
                    UnitPrice = inventory.Price
                };
                await _orderRepository.AddOrderItemAsync(orderItem);

                // ✅ تقليل الكمية
                inventory.Quantity -= item.Quantity;

                // ✅ لو الكمية وصلت 0 خليه غير متاح
                if (inventory.Quantity == 0)
                    inventory.IsAvailable = false;

                await _orderRepository.UpdateInventoryAsync(inventory);
            }

            var invoice = new Invoice
            {
                OrderId = order.ID,
                Date = DateTime.Now,
                TotalAmount = totalAmount,
                PaymentStatus = "Unpaid",
                PaymentMethod = "Cash"
            };
            await _orderRepository.CreateInvoiceAsync(invoice);

            return new OrderResultDto
            {
                OrderId = order.ID,
                TotalAmount = totalAmount,
                Status = order.Status,
                InvoiceId = invoice.ID,
                InvoiceTotalAmount = invoice.TotalAmount,
                PaymentStatus = invoice.PaymentStatus
            };
        }

        public async Task<IEnumerable<OrderResultDto>> GetPatientOrdersAsync(string patientId)
        {
            var orders = await _orderRepository.GetPatientOrdersAsync(patientId);
            return orders.Select(o => new OrderResultDto
            {
                OrderId = o.ID,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                InvoiceId = o.Invoice?.ID,
                InvoiceTotalAmount = o.Invoice?.TotalAmount ?? 0,
                PaymentStatus = o.Invoice?.PaymentStatus
            });
        }

        public async Task<OrderResultDto> UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
                return null;

            order.Status = status;
            await _orderRepository.UpdateOrderAsync(order);

            return new OrderResultDto
            {
                OrderId = order.ID,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                InvoiceId = order.Invoice?.ID,
                InvoiceTotalAmount = order.Invoice?.TotalAmount ?? 0,
                PaymentStatus = order.Invoice?.PaymentStatus
            };
        }

        public async Task<IEnumerable<OrderResultDto>> GetPharmacyOrdersAsync(string pharmacyId)
        {
            var orders = await _orderRepository.GetPharmacyOrdersAsync(pharmacyId);

            return orders.Select(o => new OrderResultDto
            {
                OrderId = o.ID,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                InvoiceId = o.Invoice?.ID,
                InvoiceTotalAmount = o.Invoice?.TotalAmount ?? 0,
                PaymentStatus = o.Invoice?.PaymentStatus
            });
        }

        public async Task<OrderResultDto> UpdateInvoicePaymentAsync(string orderId, string paymentStatus, string paymentMethod)
        {
            var invoice = await _orderRepository.GetInvoiceByOrderIdAsync(orderId);

            if (invoice == null)
                return null;

            invoice.PaymentStatus = paymentStatus;
            invoice.PaymentMethod = paymentMethod;
            await _orderRepository.UpdateInvoiceAsync(invoice);

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            return new OrderResultDto
            {
                OrderId = orderId,
                TotalAmount = invoice.TotalAmount,
                Status = order?.Status,
                InvoiceId = invoice.ID,
                InvoiceTotalAmount = invoice.TotalAmount,
                PaymentStatus = invoice.PaymentStatus
            };
        }
        public async Task<OrderResultDto> CancelOrderAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
                return null;

            // ✅ مينفعش تلغي Order مش Pending
            if (order.Status != "Pending")
                throw new Exception("مينفعش تلغي Order إلا لو حالته Pending!");

            await _orderRepository.CancelOrderAsync(order);

            return new OrderResultDto
            {
                OrderId = order.ID,
                TotalAmount = order.TotalAmount,
                Status = "Cancelled",
                InvoiceId = order.Invoice?.ID,
                InvoiceTotalAmount = order.Invoice?.TotalAmount ?? 0,
                PaymentStatus = order.Invoice?.PaymentStatus
            };
        }
    }
}