using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
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
        public  IQueryable<Inventory> GetInventoryByPharmacyIdAsync(string PharmacyId)
        {
            return _context.Inventories
                .Include(i => i.PharmacyProduct)
                .Where(i => i.PharmacyId == PharmacyId)
                ;
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
        public async Task<Pharmacy> GetPharmacyByIdWithInventoryAsync(string pharmacyId)
        {
            return await _context.Pharmacies
                    .Include(p => p.Inventories)
                        .ThenInclude(i => i.PharmacyProduct) // ✅ عشان Name و Category ميبقوش null
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
        public  IQueryable<Pharmacy> GetAllPharmacyByConfirmationStatusAsync(ConfrmationStatus status)
        {
            return _context.Pharmacies
                .Where(i => i.Status == status);
        }

        public async Task<IEnumerable<Pharmacy>> GetPendingPharmaciesAsync()
        {
            return await _context.Pharmacies
                .Where(p => p.Status == ConfrmationStatus.Pending)
                .ToListAsync();
        }

        public async Task<bool> ApprovePharmacyAsync(string pharmacyId)
        {
            var pharmacy = await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);
            if (pharmacy == null) return false;
            pharmacy.Status = ConfrmationStatus.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectPharmacyAsync(string pharmacyId)
        {
            var pharmacy = await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);
            if (pharmacy == null) return false;
            pharmacy.Status = ConfrmationStatus.Rejected;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Pharmacy>> GetAllPharmaciesAsync()
        {
            return await _context.Pharmacies.ToListAsync();
        }

        public async Task<IEnumerable<Pharmacy>> GetApprovedPharmaciesAsync()
        {
            return await _context.Pharmacies
                .Where(p => p.Status == ConfrmationStatus.Approved)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pharmacy>> GetRejectedPharmaciesAsync()
        {
            return await _context.Pharmacies
                .Where(p => p.Status == ConfrmationStatus.Rejected)
                .ToListAsync();
        }

        public async Task<bool> DeletePharmacyAsync(string pharmacyId)
        {
            var pharmacy = await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);
            if (pharmacy == null) return false;
            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<bool> DeletePatientAsync(string patientId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.ID == patientId);
            if (patient == null) return false;
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanPatientAsync(string patientId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == patientId);
            if (user == null) return false;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.ID == doctorId);
            if (doctor == null) return false;
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanDoctorAsync(string doctorId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == doctorId);
            if (user == null) return false;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> GetTotalDoctorsCountAsync()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task<int> GetTotalPharmaciesCountAsync()
        {
            return await _context.Pharmacies.CountAsync();
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.Invoices
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Patient)
                .Include(o => o.Pharmacy)
                .Include(o => o.Invoice)
                .ToListAsync();
        }
        public async Task<IEnumerable<Pharmacy>> GetNearestPharmaciesAsync(string? drugName, double latitude, double longitude, double radius)
        {
            List<Pharmacy> pharmacies;
            if (drugName is null)
            {
                pharmacies = await _context.Pharmacies
               .Where(p => p.Status == ConfrmationStatus.Approved &&
                   p.Latitude != null && p.Longitude != null)
               .ToListAsync();
            }
            pharmacies = await _context.Pharmacies
            .Where(p => p.Status == ConfrmationStatus.Approved &&
                p.Latitude != null && p.Longitude != null &&
                p.Inventories.Any(i =>
                    i.IsAvailable &&
                    i.Quantity > 0 &&
                    i.PharmacyProduct.Name.Contains(drugName)))
            .Include(p => p.Inventories)
                .ThenInclude(i => i.PharmacyProduct)
            .ToListAsync();

            return pharmacies.Where(p =>
            {
                var distance = CalculateDistance(latitude, longitude, p.Latitude!.Value, p.Longitude!.Value);
                return distance <= radius;
            })
            .OrderBy(p => CalculateDistance(latitude, longitude, p.Latitude!.Value, p.Longitude!.Value));
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // نصف قطر الأرض بالكيلومتر
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        //########################################### بتاع موقع الدكتووووور
        public async Task<IEnumerable<Clinic>> GetNearestClinicsAsync(string specialization, double latitude, double longitude, double radius)
        {
            var clinics = await _context.Clinics
                .Where(c => c.Status == ConfrmationStatus.Approved &&
                    c.Latitude != null && c.Longitude != null &&
                    c.Doctor.Specialization.Name.Contains(specialization))
                .Include(c => c.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(c => c.phones)
                .ToListAsync();

            return clinics.Where(c =>
            {
                var distance = CalculateDistance(latitude, longitude, c.Latitude!.Value, c.Longitude!.Value);
                return distance <= radius;
            })
            .OrderBy(c => CalculateDistance(latitude, longitude, c.Latitude!.Value, c.Longitude!.Value));
        }
        public async Task<bool> UpdatePharmacyLocationAsync(string pharmacyId, double latitude, double longitude)
        {
            var pharmacy = await _context.Pharmacies
                .FirstOrDefaultAsync(p => p.ID == pharmacyId);
            if (pharmacy == null) return false;
            pharmacy.Latitude = latitude;
            pharmacy.Longitude = longitude;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<decimal> GetDailySalesAsync(string pharmacyId)
        {
            var today = DateTime.Today;
            return await _context.Invoices
                .Where(i => i.Order.PharmacyId == pharmacyId &&
                       i.Date >= today && i.Date < today.AddDays(1))
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<decimal> GetWeeklySalesAsync(string pharmacyId)
        {
            var weekAgo = DateTime.Today.AddDays(-7);
            return await _context.Invoices
                .Where(i => i.Order.PharmacyId == pharmacyId &&
                       i.Date >= weekAgo)
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<decimal> GetMonthlySalesAsync(string pharmacyId)
        {
            var monthAgo = DateTime.Today.AddDays(-30);
            return await _context.Invoices
                .Where(i => i.Order.PharmacyId == pharmacyId &&
                       i.Date >= monthAgo)
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<IEnumerable<TopDrugDto>> GetTopDrugsAsync(string pharmacyId)
        {
            return await _context.OrderItems
                .Where(oi => oi.Order.PharmacyId == pharmacyId)
                .GroupBy(oi => oi.Inventory.PharmacyProduct.Name)
                .Select(g => new TopDrugDto
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<PeakHoursDto>> GetPeakHoursAsync(string pharmacyId)
        {
            return await _context.Orders
                .Where(o => o.PharmacyId == pharmacyId)
                .GroupBy(o => o.Date.Hour)
                .Select(g => new PeakHoursDto
                {
                    Hour = g.Key,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalOrders)
                .ToListAsync();
        }


    }
}