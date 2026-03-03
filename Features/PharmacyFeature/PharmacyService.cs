using DataAccess.Repositry.IRepositry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;
using Models;

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
                    ProductName = i.PharmacyProduct.Name
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

        public async Task<PharmacyProduct> AddProductAsync(AddProductDto dto)
        {
            var product = new PharmacyProduct
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category
            };
            await _pharmacyRepository.AddProductAsync(product);
            return product;
        }

        public async Task<Inventory> AddInventoryAsync(AddInventoryDto dto)
        {
            var inventory = new Inventory
            {
                PharmacyId = dto.PharmacyId,
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
                    ProductName = i.PharmacyProduct.Name
                }));
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

        public async Task<bool> UpdatePharmacyStatusAsync(string pharmacyId, string status)
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
    }
}