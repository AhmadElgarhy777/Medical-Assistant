using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry.IRepositry
{
    public interface IPharmacyRepository
    {
        Task<IEnumerable<Pharmacy>> SearchByDrugNameAsync(string drugName);
        Task AddPharmacyAsync(Pharmacy pharmacy);
        Task AddProductAsync(PharmacyProduct product);
        Task AddInventoryAsync(Inventory inventory);
        Task<IEnumerable<Pharmacy>> GetByCategoryAsync(string category);
        Task UpdateMedicineAsync(Inventory inventory);
        Task UpdateStockAsync(string inventoryId, int newQuantity);
        Task<bool> DeleteMedicineAsync(string inventoryId);
        Task<Inventory> GetInventoryByIdAsync(string inventoryId); // ✅ زود السطر ده
    }
}