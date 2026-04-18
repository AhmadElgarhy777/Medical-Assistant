using DataAccess.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;
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
                    Governorate = p.Governorate,
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
                    Governorate = p.Governorate,
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



        public async Task<IEnumerable<DrugDTO>> SearchInSpecificPharmacyAsync(string pharmacyId, string drugNameOrCategory)
        {
            var pharmacy = await _pharmacyRepository.GetPharmacyByIdAsync(pharmacyId);
            if (pharmacy == null)
                return Enumerable.Empty<DrugDTO>();

            var results = pharmacy.Inventories
                .Where(i => i.IsAvailable && i.Quantity > 0 &&
                    (i.PharmacyProduct.Name.Contains(drugNameOrCategory) ||
                     i.PharmacyProduct.Category.Contains(drugNameOrCategory)))
                .Select(i => new DrugDTO
                {
                    Name = i.PharmacyProduct.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    Category = i.PharmacyProduct.Category,
                    InvetoryId = i.ID
                });
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
                Governorate = pharmacy.Governorate,
                City = pharmacy.City,
                RealImg = pharmacy.RealImg,
                PharmacyLicense = pharmacy.PharmacyLicense,
            };
        }
    }
}