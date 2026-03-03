using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Features.PharmacyFeature;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;

        public PharmacyController(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        // ✅ أي حد يقدر يبحث
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchByDrugName([FromQuery] string drugName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(drugName))
                return BadRequest("ادخل اسم الدواء!");

            var result = await _pharmacyService.SearchByDrugNameAsync(drugName, pageNumber, pageSize);
            if (!result.Data.Any())
                return NotFound("مفيش صيدليات عندها الدواء ده!");

            return Ok(result);
        }

        // ✅ أي حد يقدر يبحث بالكاتيجوري
        [HttpGet("ByCategory/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var result = await _pharmacyService.GetByCategoryAsync(category);
            if (!result.Any())
                return NotFound("مفيش صيدليات عندها أدوية في الكاتيجوري دي!");

            return Ok(result);
        }

        // ✅ الأدمن بس يضيف صيدلية
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPharmacy([FromBody] AddPharmacyDto dto)
        {
            var result = await _pharmacyService.AddPharmacyAsync(dto);
            return Ok(result);
        }

        // ✅ الصيدلية بس تضيف دواء
        [HttpPost("product/add")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            var result = await _pharmacyService.AddProductAsync(dto);
            return Ok(result);
        }

        // ✅ الصيدلية بس تضيف Inventory
        [HttpPost("inventory/add")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> AddInventory([FromBody] AddInventoryDto dto)
        {
            var result = await _pharmacyService.AddInventoryAsync(dto);
            return Ok(result);
        }

        // ✅ الصيدلية بس تعدل دواء
        [HttpPut("UpdateMedicine/{id}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateMedicine(string id, [FromQuery] decimal price, [FromQuery] string productName, [FromQuery] string? category)
        {
            var result = await _pharmacyService.UpdateMedicineAsync(id, price, productName, category);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تعديل الدواء بنجاح!");
        }

        // ✅ الصيدلية بس تحدث الكمية
        [HttpPatch("UpdateStock/{id}/{newQuantity}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateStock(string id, int newQuantity)
        {
            var result = await _pharmacyService.UpdateStockAsync(id, newQuantity);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تحديث الكمية بنجاح!");
        }

        // ✅ الصيدلية بس تحذف دواء
        [HttpDelete("DeleteMedicine/{id}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> DeleteMedicine(string id)
        {
            var result = await _pharmacyService.DeleteMedicineAsync(id);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم حذف الدواء بنجاح!");
        }

        // ✅ الصيدلية تغير حالتها
        [HttpPatch("status/{pharmacyId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdatePharmacyStatus(string pharmacyId, [FromQuery] string status)
        {
            var validStatuses = new[] { "Active", "Inactive" };
            if (!validStatuses.Contains(status))
                return BadRequest("الحالة مش صحيحة! لازم تكون: Active أو Inactive");

            await _pharmacyService.UpdatePharmacyStatusAsync(pharmacyId, status);
            return Ok($"تم تغيير حالة الصيدلية لـ {status}");
        }

        // ✅ الصيدلية تشوف الأدوية على وشك النفاد
        [HttpGet("lowstock/{pharmacyId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> GetLowStock(string pharmacyId, [FromQuery] int threshold = 10)
        {
            var result = await _pharmacyService.GetLowStockAsync(pharmacyId, threshold);

            if (!result.Any())
                return NotFound("مفيش أدوية على وشك النفاد!");

            return Ok(result);
        }
    }
}