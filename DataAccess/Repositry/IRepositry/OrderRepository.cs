using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repositry.IRepositry
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Inventory> GetInventoryByIdAsync(string inventoryId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.ID == inventoryId);
        }

        public async Task<IEnumerable<Order>> GetPatientOrdersAsync(string patientId)
        {
            return await _context.Orders
                .Where(o => o.PatientId == patientId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Inventory)
                        .ThenInclude(i => i.PharmacyProduct)
                .Include(o => o.Invoice)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.Invoice)
                .FirstOrDefaultAsync(o => o.ID == orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> GetPharmacyOrdersAsync(string pharmacyId)
        {
            return await _context.Orders
                .Where(o => o.PharmacyId == pharmacyId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Inventory)
                        .ThenInclude(i => i.PharmacyProduct)
                .Include(o => o.Invoice)
                .ToListAsync();
        }
        public async Task<Invoice> GetInvoiceByOrderIdAsync(string orderId)
        {
            return await _context.Invoices
                .FirstOrDefaultAsync(i => i.OrderId == orderId);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }
        public async Task CancelOrderAsync(Order order)
        {
            order.Status = "Cancelled";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}