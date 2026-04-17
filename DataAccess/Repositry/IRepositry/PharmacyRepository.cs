using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry.IRepositry
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly ApplicationDbContext _context;

        public PharmacyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pharmacy>> SearchByDrugNameAsync(string drugName)
        {
            return await _context.Pharmacies
                .Where(p => p.Status == ConfrmationStatus.Approved &&
                    p.Inventories.Any(i =>
                        i.IsAvailable &&
                        i.Quantity > 0 &&
                        i.PharmacyProduct.Name.Contains(drugName)))
                .Include(p => p.Inventories)
                    .ThenInclude(i => i.PharmacyProduct)
                .ToListAsync();
        }

        public async Task AddPharmacyAsync(Pharmacy pharmacy)
        {
            await _context.Pharmacies.AddAsync(pharmacy);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductAsync(PharmacyProduct product)
        {
            await _context.PharmacyProducts.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(string inventoryId)
        {
            return await _context.Inventories
                .Include(i => i.PharmacyProduct)
                .FirstOrDefaultAsync(i => i.ID == inventoryId);
        }

        public async Task<IEnumerable<Pharmacy>> GetByCategoryAsync(string category)
        {
            return await _context.Pharmacies
                .Where(p => p.Status == ConfrmationStatus.Approved &&
                    p.Inventories.Any(i =>
                        i.IsAvailable &&
                        i.Quantity > 0 &&
                        i.PharmacyProduct.Category == category))
                .Include(p => p.Inventories)
                    .ThenInclude(i => i.PharmacyProduct)
                .ToListAsync();
        }

        public async Task UpdateMedicineAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(string inventoryId, int newQuantity)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ID == inventoryId);

            if (inventory != null)
            {
                inventory.Quantity = newQuantity;
                inventory.IsAvailable = newQuantity > 0;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteMedicineAsync(string inventoryId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ID == inventoryId);

            if (inventory == null)
                return false;

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdatePharmacyStatusAsync(string pharmacyId, ConfrmationStatus status)
        {
            var pharmacy = await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);

            if (pharmacy != null)
            {
                pharmacy.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inventory>> GetLowStockAsync(string pharmacyId, int threshold)
        {
            return await _context.Inventories
                .Where(i => i.PharmacyId == pharmacyId && i.Quantity <= threshold)
                .Include(i => i.PharmacyProduct)
                .ToListAsync();
        }

        public async Task<Pharmacy> GetPharmacyByIdAsync(string pharmacyId)
        {
            return await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);
        }

        public async Task UpdatePharmacyAsync(Pharmacy pharmacy)
        {
            _context.Pharmacies.Update(pharmacy);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPendingOrdersCountAsync(string pharmacyId)
        {
            return await _context.Orders
                .Where(o => o.PharmacyId == pharmacyId && o.Status == "Pending")
                .CountAsync();
        }

        public async Task<int> GetLowStockCountAsync(string pharmacyId, int threshold = 10)
        {
            return await _context.Inventories
                .Where(i => i.PharmacyId == pharmacyId && i.Quantity <= threshold)
                .CountAsync();
        }

        public async Task<decimal> GetTodaySalesAsync(string pharmacyId)
        {
            var today = DateTime.Today;
            return await _context.Invoices
                .Where(i => i.Order.PharmacyId == pharmacyId &&
                       i.Date >= today && i.Date < today.AddDays(1))
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<int> GetTotalInventoryAsync(string pharmacyId)
        {
            return await _context.Inventories
                .Where(i => i.PharmacyId == pharmacyId)
                .CountAsync();
        }

        public async Task AddRatingAsync(string pharmacyId, string patientId, int rating, string comment)
        {
            var newRating = new PharmacyRating
            {
                PharmacyId = pharmacyId,
                PatientId = patientId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };
            await _context.PharmacyRatings.AddAsync(newRating);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingAsync(string pharmacyId)
        {
            var ratings = await _context.PharmacyRatings
                .Where(r => r.PharmacyId == pharmacyId)
                .ToListAsync();

            if (!ratings.Any()) return 0;
            return ratings.Average(r => r.Rating);
        }
        public async Task<IEnumerable<Inventory>> GetPharmacyInventoryAsync(string pharmacyId)
        {
            return await _context.Inventories
                .Where(i => i.PharmacyId == pharmacyId)
                .Include(i => i.PharmacyProduct)
                .ToListAsync();
        }

    }
}