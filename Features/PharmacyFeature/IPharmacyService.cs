using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;
using Models;

namespace Features.PharmacyFeature
{
    public interface IPharmacyService
    {
       // Task<IEnumerable<PharmacyResultDto>> SearchByDrugNameAsync(string drugName);
        Task<Pharmacy> AddPharmacyAsync(AddPharmacyDto dto);
        Task<PharmacyProduct> AddProductAsync(AddProductDto dto);
        Task<Inventory> AddInventoryAsync(AddInventoryDto dto);
        Task<IEnumerable<PharmacyResultDto>> GetByCategoryAsync(string category);
        Task<bool> UpdateMedicineAsync(string inventoryId, decimal price, string productName, string? category);
        Task<bool> UpdateStockAsync(string inventoryId, int newQuantity);
        Task<bool> DeleteMedicineAsync(string inventoryId);
        Task<bool> UpdatePharmacyStatusAsync(string pharmacyId, string status);
        Task<IEnumerable<LowStockDto>> GetLowStockAsync(string pharmacyId, int threshold);
        Task<PagedResultDto<PharmacyResultDto>> SearchByDrugNameAsync(string drugName, int pageNumber = 1, int pageSize = 10);
    }
}