using DataAccess.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PharmacyFeature
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IPharmacyRepository _pharmacyRepository;

        public PharmacyService(IPharmacyRepository pharmacyRepository)
        {
            _pharmacyRepository = pharmacyRepository;
        }

        public async Task<PagedResultDto<PharmacyResultDto>> SearchByDrugNameAsync(string drugName, int pageNumber = 1, int pageSize = 10)
        {
            var pharmacies = await _pharmacyRepository.SearchByDrugNameAsync(drugName);

            var results = pharmacies.SelectMany(p => p.Inventories
                .Where(i => i.IsAvailable && i.Quantity > 0 &&
                    i.PharmacyProduct.Name.Contains(drugName))
                .Select(i => new PharmacyResultDto
                {
                    PharmacyId = p.ID,
                    PharmacyName = p.Name,
                    Address = p.Address,
                    City = p.City,
                    Governorate = p.Governorate.ToString(),
                    Phone = p.Phone,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ProductName = i.PharmacyProduct.Name,
                    RattingAverage = p.RattingAverage
                })).ToList();

            var totalCount = results.Count;
            var data = results
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResultDto<PharmacyResultDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = data
            };
        }

        public async Task<Pharmacy> AddPharmacyAsync(AddPharmacyDto dto)
        {
            var pharmacy = new Pharmacy
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Governorate = dto.Governorate,
                City = dto.City,
                Status = dto.Status,
                Gender = dto.Gender,
                PharmacyLicense = dto.PharmacyLicense,
                RealImg = dto.RealImg,
                BD = dto.BD
            };
            await _pharmacyRepository.AddPharmacyAsync(pharmacy);
            return pharmacy;
        }

        public async Task<PharmacyProduct> AddProductAsync(string pharmacyId, AddProductDto dto)
        {

            var product = new PharmacyProduct
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                PharmacyId = pharmacyId

            };
            await _pharmacyRepository.AddProductAsync(product);
            return product;
        }

        public async Task<Inventory> AddInventoryAsync(string pharmacyId, AddInventoryDto dto)
        {
            var inventory = new Inventory
            {
                PharmacyId = pharmacyId,
                PharmacyProductId = dto.PharmacyProductId,
                Price = dto.Price,
                Quantity = dto.Quantity,
                IsAvailable = dto.IsAvailable
            };
            await _pharmacyRepository.AddInventoryAsync(inventory);
            return inventory;
        }

        public async Task<IEnumerable<PharmacyResultDto>> GetByCategoryAsync(string category)
        {
            var pharmacies = await _pharmacyRepository.GetByCategoryAsync(category);

            return pharmacies.SelectMany(p => p.Inventories
                .Where(i => i.IsAvailable && i.Quantity > 0 &&
                    i.PharmacyProduct.Category == category)
                .Select(i => new PharmacyResultDto
                {
                    PharmacyId = p.ID,
                    PharmacyName = p.Name,
                    Address = p.Address,
                    City = p.City,
                    Governorate = p.Governorate.ToString(),
                    Phone = p.Phone,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ProductName = i.PharmacyProduct.Name,
                    RattingAverage = p.RattingAverage
                }));
        }

         public List<MedicineInvetoryListDTO> GetAllPharmacyInvetoryMedicine(string PharmacyId)
        {
            var inventories = _pharmacyRepository.GetInventoryByPharmacyIdAsync(PharmacyId);
            var result = inventories.Select(i => new MedicineInvetoryListDTO
            {
                InvetoryId = i.ID,
                MedicineName = i.PharmacyProduct.Name,
                MedicineCategory = i.PharmacyProduct.Category,
                Price = i.Price,
                Quantity = i.Quantity,
                IsAvailable = i.IsAvailable
            }).ToList();
            return result;  

        }

        public async Task<bool> UpdateMedicineAsync(string inventoryId, decimal price, string productName, string? category)
        {
            var inventory = await _pharmacyRepository.GetInventoryByIdAsync(inventoryId);

            if (inventory == null)
                return false;

            inventory.Price = price;
            inventory.PharmacyProduct.Name = productName;
            inventory.PharmacyProduct.Category = category;
            await _pharmacyRepository.UpdateMedicineAsync(inventory);
            return true;
        }

        public async Task<bool> UpdateStockAsync(string inventoryId, int newQuantity)
        {
            await _pharmacyRepository.UpdateStockAsync(inventoryId, newQuantity);
            return true;
        }

        public async Task<bool> DeleteMedicineAsync(string inventoryId)
        {
            return await _pharmacyRepository.DeleteMedicineAsync(inventoryId);
        }

        public async Task<bool> UpdatePharmacyStatusAsync(string pharmacyId, ConfrmationStatus status)
        {
            await _pharmacyRepository.UpdatePharmacyStatusAsync(pharmacyId, status);
            return true;
        }

        public async Task<IEnumerable<LowStockDto>> GetLowStockAsync(string pharmacyId, int threshold)
        {
            var inventories = await _pharmacyRepository.GetLowStockAsync(pharmacyId, threshold);

            return inventories.Select(i => new LowStockDto
            {
                InventoryId = i.ID,
                ProductName = i.PharmacyProduct.Name,
                Category = i.PharmacyProduct.Category,
                Quantity = i.Quantity,
                Price = i.Price
            });

        }
        public async Task<Pharmacy> GetPharmacyByIdAsync(string pharmacyId)
        {
            return await _pharmacyRepository.GetPharmacyByIdAsync(pharmacyId);
        }

        public async Task<bool> UpdatePharmacyInfoAsync(string pharmacyId, string name, string address, string phone, string city, string governorate)
        {
            var pharmacy = await _pharmacyRepository.GetPharmacyByIdAsync(pharmacyId);

            if (pharmacy == null)
                return false;

            pharmacy.Name = name;
            pharmacy.Address = address;
            pharmacy.Phone = phone;
            pharmacy.City = city;
            pharmacy.Governorate = governorate;

            await _pharmacyRepository.UpdatePharmacyAsync(pharmacy);
            return true;
        }
        public async Task<PharmacyDashboardDto> GetDashboardAsync(string pharmacyId)
        {
            var pendingOrders = await _pharmacyRepository.GetPendingOrdersCountAsync(pharmacyId);
            var lowStockCount = await _pharmacyRepository.GetLowStockCountAsync(pharmacyId);
            var todaySales = await _pharmacyRepository.GetTodaySalesAsync(pharmacyId);
            var totalInventory = await _pharmacyRepository.GetTotalInventoryAsync(pharmacyId);
            var averageRating = await _pharmacyRepository.GetAverageRatingAsync(pharmacyId);

            return new PharmacyDashboardDto
            {
                PendingOrders = pendingOrders,
                LowStockCount = lowStockCount,
                TodaySales = todaySales,
                TotalInventory = totalInventory,
                AverageRating = averageRating
            };
        }

        public async Task AddRatingAsync(string pharmacyId, string patientId, int rating, string comment)
        {
            await _pharmacyRepository.AddRatingAsync(pharmacyId, patientId, rating, comment);
        }

        public async Task<double> GetAverageRatingAsync(string pharmacyId)
        {
            return await _pharmacyRepository.GetAverageRatingAsync(pharmacyId);
        }
        public async Task<IEnumerable<Inventory>> GetPharmacyInventoryAsync(string pharmacyId)
        {
            return await _pharmacyRepository.GetPharmacyInventoryAsync(pharmacyId);
        }



        public async Task<IEnumerable<Inventory>> SearchInSpecificPharmacyAsync(string pharmacyId, string drugNameOrCategory)
        {
            var pharmacy = await _pharmacyRepository.GetPharmacyByIdWithInventoryAsync(pharmacyId);
            if (pharmacy == null)
                return Enumerable.Empty<Inventory>();

            // ✅ تأكد إن Inventories مش null
            if (pharmacy.Inventories == null)
                return Enumerable.Empty<Inventory>();

            var results = pharmacy.Inventories
                .Where(i => i.IsAvailable && i.Quantity > 0 &&
                    (i.PharmacyProduct.Name.Contains(drugNameOrCategory) ||
                     i.PharmacyProduct.Category.Contains(drugNameOrCategory)));

            return results;
        }

       
        public async Task<PharmacyProfileDTO> GetPharmacyProfileAsync(string pharmacyId)
        {
            var pharmacy = await _pharmacyRepository.GetPharmacyByIdAsync(pharmacyId);
            if (pharmacy == null)
                return null;

            return new PharmacyProfileDTO
            {
                ID = pharmacy.ID,
                Name = pharmacy.Name,
                Address = pharmacy.Address,
                Phone = pharmacy.Phone,
                Governorate = pharmacy.Governorate.ToString(),
                City = pharmacy.City,
                RealImg = pharmacy.RealImg,
                PharmacyLicense = pharmacy.PharmacyLicense,
            };
        }
        public async Task<IEnumerable<Pharmacy>> GetPendingPharmaciesAsync()
        {
            return await _pharmacyRepository.GetPendingPharmaciesAsync();
        }

        public async Task<bool> ApprovePharmacyAsync(string pharmacyId)
        {
            return await _pharmacyRepository.ApprovePharmacyAsync(pharmacyId);
        }

        public async Task<bool> RejectPharmacyAsync(string pharmacyId)
        {
            return await _pharmacyRepository.RejectPharmacyAsync(pharmacyId);
        }
        public async Task<IEnumerable<Pharmacy>> GetAllPharmaciesAsync()
        {
            return await _pharmacyRepository.GetAllPharmaciesAsync();
        }

        public async Task<IEnumerable<Pharmacy>> GetApprovedPharmaciesAsync()
        {
            return await _pharmacyRepository.GetApprovedPharmaciesAsync();
        }

        public async Task<IEnumerable<Pharmacy>> GetRejectedPharmaciesAsync()
        {
            return await _pharmacyRepository.GetRejectedPharmaciesAsync();
        }

        public async Task<bool> DeletePharmacyAsync(string pharmacyId)
        {
            return await _pharmacyRepository.DeletePharmacyAsync(pharmacyId);
        }
        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _pharmacyRepository.GetAllPatientsAsync();
        }

        public async Task<bool> DeletePatientAsync(string patientId)
        {
            return await _pharmacyRepository.DeletePatientAsync(patientId);
        }

        public async Task<bool> BanPatientAsync(string patientId)
        {
            return await _pharmacyRepository.BanPatientAsync(patientId);
        }
        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _pharmacyRepository.GetAllDoctorsAsync();
        }

        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            return await _pharmacyRepository.DeleteDoctorAsync(doctorId);
        }

        public async Task<bool> BanDoctorAsync(string doctorId)
        {
            return await _pharmacyRepository.BanDoctorAsync(doctorId);
        }

        public async Task<SuperAdminStatsDto> GetStatsAsync()
        {
            return new SuperAdminStatsDto
            {
                TotalPatients = await _pharmacyRepository.GetTotalPatientsCountAsync(),
                TotalDoctors = await _pharmacyRepository.GetTotalDoctorsCountAsync(),
                TotalPharmacies = await _pharmacyRepository.GetTotalPharmaciesCountAsync(),
                TotalOrders = await _pharmacyRepository.GetTotalOrdersCountAsync(),
                TotalSales = await _pharmacyRepository.GetTotalSalesAsync()
            };
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _pharmacyRepository.GetAllOrdersAsync();
        }
        public async Task<IEnumerable<NearestPharmacyDto>> GetNearestPharmaciesAsync(string? drugName, double latitude, double longitude, double radius)
        {
            var pharmacies = await _pharmacyRepository.GetNearestPharmaciesAsync(drugName, latitude, longitude, radius);

            return pharmacies.Select(p =>
            {
                var inventory = p.Inventories.FirstOrDefault(i => i.PharmacyProduct.Name.Contains(drugName));
                var distance = CalculateDistance(latitude, longitude, p.Latitude!.Value, p.Longitude!.Value);

                return new NearestPharmacyDto
                {
                    PharmacyId = p.ID,
                    PharmacyName = p.Name,
                    Address = p.Address,
                    Phone = p.Phone,
                    Distance = Math.Round(distance, 2),
                    Price = inventory?.Price ?? 0,
                    Quantity = inventory?.Quantity ?? 0,
                    ProductName = inventory?.PharmacyProduct?.Name ?? ""
                };
            })
            .OrderBy(p => p.Distance);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        //#################################################### يدكتووووووور
        public async Task<IEnumerable<NearestClinicDto>> GetNearestClinicsAsync(string specialization, double latitude, double longitude, double radius)
        {
            var clinics = await _pharmacyRepository.GetNearestClinicsAsync(specialization, latitude, longitude, radius);

            return clinics.Select(c =>
            {
                var distance = CalculateDistance(latitude, longitude, c.Latitude!.Value, c.Longitude!.Value);

                return new NearestClinicDto
                {
                    ClinicId = c.ID,
                    DoctorName = c.Doctor.FullName,
                    Specialization = c.Doctor.Specialization.Name,
                    Address = c.Address,
                    City = c.City,
                    Price = c.price,
                    Distance = Math.Round(distance, 2),
                    DoctorRating = c.Doctor.RattingAverage
                };
            })
            .OrderBy(c => c.Distance);
        }
        public async Task<bool> UpdatePharmacyLocationAsync(string pharmacyId, double latitude, double longitude)
        {
            return await _pharmacyRepository.UpdatePharmacyLocationAsync(pharmacyId, latitude, longitude);
        }
        public async Task<SalesReportDto> GetSalesReportAsync(string pharmacyId)
        {
            return new SalesReportDto
            {
                DailySales = await _pharmacyRepository.GetDailySalesAsync(pharmacyId),
                WeeklySales = await _pharmacyRepository.GetWeeklySalesAsync(pharmacyId),
                MonthlySales = await _pharmacyRepository.GetMonthlySalesAsync(pharmacyId),
                TopDrugs = await _pharmacyRepository.GetTopDrugsAsync(pharmacyId),
                PeakHours = await _pharmacyRepository.GetPeakHoursAsync(pharmacyId)
            };
        }
    }
}