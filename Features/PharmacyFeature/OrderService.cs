using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using Models;
using Models.DTOs;
using Models.Enums;

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
                Status = OrderStatusEnum.Pending,
                TotalAmount = totalAmount
            };
            await _orderRepository.CreateOrderAsync(order);

            foreach (var item in dto.Items)
            {
                var inventory = await _orderRepository.GetInventoryByIdAsync(item.InventoryId);

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

                inventory.Quantity -= item.Quantity;

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
                PaymentStatus = o.Invoice?.PaymentStatus,
                PharmacyName = o.Pharmacy?.Name ?? "صيدلية",
                Items = o.OrderItems.Select(oi => new OrderItemResultDto
                {
                    MedicineName = oi.MedicineName ?? oi.Inventory?.PharmacyProduct?.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            });
        }

        public async Task<OrderResultDto> UpdateOrderStatusAsync(string orderId, OrderStatusEnum status)
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
                PaymentStatus = order.Invoice?.PaymentStatus,
                PharmacyName = order.Pharmacy?.Name ?? "صيدلية",
                Items = order.OrderItems?.Select(oi => new OrderItemResultDto
                {
                    MedicineName = oi.MedicineName ?? oi.Inventory?.PharmacyProduct?.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList() ?? new List<OrderItemResultDto>()
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
                PaymentStatus = o.Invoice?.PaymentStatus,
                PharmacyName = o.Pharmacy?.Name ?? "صيدلية",
                Items = o.OrderItems?.Select(oi => new OrderItemResultDto
                {
                    MedicineName = oi.MedicineName ?? oi.Inventory?.PharmacyProduct?.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList() ?? new List<OrderItemResultDto>()
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
                Status = order.Status,
                InvoiceId = invoice.ID,
                InvoiceTotalAmount = invoice.TotalAmount,
                PaymentStatus = invoice.PaymentStatus,
                PharmacyName = order.Pharmacy?.Name ?? "صيدلية",
                Items = order.OrderItems?.Select(oi => new OrderItemResultDto
                {
                    MedicineName = oi.MedicineName ?? oi.Inventory?.PharmacyProduct?.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList() ?? new List<OrderItemResultDto>()
            };
        }

        public async Task<OrderResultDto> CancelOrderAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
                return null;

            if (order.Status != OrderStatusEnum.Pending)
                throw new Exception("مينفعش تلغي Order إلا لو حالته Pending!");

            foreach (var item in order.OrderItems)
            {
                var inventory = await _orderRepository.GetInventoryByIdAsync(item.InventoryId);
                inventory.Quantity += item.Quantity;
                inventory.IsAvailable = true;
                await _orderRepository.UpdateInventoryAsync(inventory);
            }

            await _orderRepository.CancelOrderAsync(order);

            return new OrderResultDto
            {
                OrderId = order.ID,
                TotalAmount = order.TotalAmount,
                Status = OrderStatusEnum.Canceled,
                InvoiceId = order.Invoice?.ID,
                InvoiceTotalAmount = order.Invoice?.TotalAmount ?? 0,
                PaymentStatus = order.Invoice?.PaymentStatus,
                PharmacyName = order.Pharmacy?.Name ?? "صيدلية",
                Items = order.OrderItems?.Select(oi => new OrderItemResultDto
                {
                    MedicineName = oi.MedicineName ?? oi.Inventory?.PharmacyProduct?.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList() ?? new List<OrderItemResultDto>()
            };
        }

        public async Task<IEnumerable<Order>> GetPharmacyOrdersByStatusAsync(string pharmacyId, OrderStatusEnum status)
        {
            return await _orderRepository.GetPharmacyOrdersByStatusAsync(pharmacyId, status);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }


        public async Task<Invoice> GetInvoiceByIdAsync(string invoiceId)
        {
            return await _orderRepository.GetInvoiceByIdAsync(invoiceId);
        }

    }
}