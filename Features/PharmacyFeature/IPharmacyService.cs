using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;
using Models;
using Models.Enums;

namespace Features.PharmacyFeature
{
    public interface IPharmacyService
    {
        // Task<IEnumerable<PharmacyResultDto>> SearchByDrugNameAsync(string drugName);
        Task<Pharmacy> AddPharmacyAsync(AddPharmacyDto dto);
        Task<PharmacyProduct> AddProductAsync(string pharmacyId, AddProductDto dto);
        Task<Inventory> AddInventoryAsync(string pharmacyId, AddInventoryDto dto);
        Task<IEnumerable<PharmacyResultDto>> GetByCategoryAsync(string category);
        List<MedicineInvetoryListDTO> GetAllPharmacyInvetoryMedicine(string PharmacyId);
        Task<bool> UpdateMedicineAsync(string inventoryId, decimal price, string productName, string? category);
        Task<bool> UpdateStockAsync(string inventoryId, int newQuantity);
        Task<bool> DeleteMedicineAsync(string inventoryId);
        Task<bool> UpdatePharmacyStatusAsync(string pharmacyId, ConfrmationStatus status);
        Task<IEnumerable<LowStockDto>> GetLowStockAsync(string pharmacyId, int threshold);
        Task<PagedResultDto<PharmacyResultDto>> SearchByDrugNameAsync(string drugName, int pageNumber = 1, int pageSize = 10);
        Task<Pharmacy> GetPharmacyByIdAsync(string pharmacyId);
        Task<bool> UpdatePharmacyInfoAsync(string pharmacyId, string name, string address, string phone, string city, string governorate);
        Task<PharmacyDashboardDto> GetDashboardAsync(string pharmacyId);
        Task AddRatingAsync(string pharmacyId, string patientId, int rating, string comment);
        Task<double> GetAverageRatingAsync(string pharmacyId);
        Task<IEnumerable<Inventory>> GetPharmacyInventoryAsync(string pharmacyId);

        Task<IEnumerable<DrugDTO>> SearchInSpecificPharmacyAsync(string pharmacyId, string drugNameOrCategory);
        // Task<IEnumerable<Pharmacy>> GetPendingPharmaciesAsync();
        // Task<bool> ApprovePharmacyAsync(string pharmacyId);
        //Task<bool> RejectPharmacyAsync(string pharmacyId);
    }
}