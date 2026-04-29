using Models;
using Models.DTOs;
using Models.Enums;
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
        Task<Inventory> GetInventoryByIdAsync(string inventoryId);
        IQueryable<Inventory> GetInventoryByPharmacyIdAsync(string PharmacyId);
        Task UpdatePharmacyStatusAsync(string pharmacyId, ConfrmationStatus status);
        Task<IEnumerable<Inventory>> GetLowStockAsync(string pharmacyId, int threshold);
        Task<Pharmacy> GetPharmacyByIdAsync(string pharmacyId);
        Task UpdatePharmacyAsync(Pharmacy pharmacy);
        // Dashboard
        Task<int> GetPendingOrdersCountAsync(string pharmacyId);
        Task<int> GetLowStockCountAsync(string pharmacyId, int threshold = 10);
        Task<decimal> GetTodaySalesAsync(string pharmacyId);
        Task<int> GetTotalInventoryAsync(string pharmacyId);

        // Rating
        Task AddRatingAsync(string pharmacyId, string patientId, int rating, string comment);
        Task<double> GetAverageRatingAsync(string pharmacyId);
        Task<IEnumerable<Inventory>> GetPharmacyInventoryAsync(string pharmacyId);
        IQueryable<Pharmacy> GetAllPharmacyByConfirmationStatusAsync(ConfrmationStatus status);
        Task<Pharmacy> GetPharmacyByIdWithInventoryAsync(string pharmacyId);


        //  Task<IEnumerable<Pharmacy>> GetPendingPharmaciesAsync();
        //  Task<bool> ApprovePharmacyAsync(string pharmacyId);
        //  Task<bool> RejectPharmacyAsync(string pharmacyId);
        // Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        //Task<bool> BanUserAsync(string userId);

        Task<IEnumerable<Pharmacy>> GetPendingPharmaciesAsync();
        Task<bool> ApprovePharmacyAsync(string pharmacyId);
        Task<bool> RejectPharmacyAsync(string pharmacyId);
        Task<IEnumerable<Pharmacy>> GetAllPharmaciesAsync();
        Task<IEnumerable<Pharmacy>> GetApprovedPharmaciesAsync();
        Task<IEnumerable<Pharmacy>> GetRejectedPharmaciesAsync();
        Task<bool> DeletePharmacyAsync(string pharmacyId);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<bool> DeletePatientAsync(string patientId);
        Task<bool> BanPatientAsync(string patientId);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<bool> DeleteDoctorAsync(string doctorId);
        Task<bool> BanDoctorAsync(string doctorId);
        Task<int> GetTotalPatientsCountAsync();
        Task<int> GetTotalDoctorsCountAsync();
        Task<int> GetTotalPharmaciesCountAsync();
        Task<int> GetTotalOrdersCountAsync();
        Task<decimal> GetTotalSalesAsync();
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<Pharmacy>> GetNearestPharmaciesAsync(string? drugName, double latitude, double longitude, double radius);
        Task<IEnumerable<Clinic>> GetNearestClinicsAsync(string specialization, double latitude, double longitude, double radius);
        Task<bool> UpdatePharmacyLocationAsync(string pharmacyId, double latitude, double longitude);
        Task<decimal> GetDailySalesAsync(string pharmacyId);
        Task<decimal> GetWeeklySalesAsync(string pharmacyId);
        Task<decimal> GetMonthlySalesAsync(string pharmacyId);
        Task<IEnumerable<TopDrugDto>> GetTopDrugsAsync(string pharmacyId);
        Task<IEnumerable<PeakHoursDto>> GetPeakHoursAsync(string pharmacyId);

    }
}