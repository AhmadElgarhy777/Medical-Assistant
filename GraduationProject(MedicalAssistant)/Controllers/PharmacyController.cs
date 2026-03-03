using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // ─── Search ───
        [HttpGet("search")]
        public async Task<IActionResult> SearchByDrugName([FromQuery] string drugName)
        {
            if (string.IsNullOrEmpty(drugName))
                return BadRequest("ادخل اسم الدواء!");

            var result = await _pharmacyService.SearchByDrugNameAsync(drugName);

            if (!result.Any())
                return NotFound("مفيش صيدليات عندها الدواء ده!");

            return Ok(result);
        }

        // ─── Add Pharmacy ───
        [HttpPost("add")]
        public async Task<IActionResult> AddPharmacy([FromBody] AddPharmacyDto dto)
        {
            var result = await _pharmacyService.AddPharmacyAsync(dto);
            return Ok(result);
        }

        // ─── Add Product ───
        [HttpPost("product/add")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            var result = await _pharmacyService.AddProductAsync(dto);
            return Ok(result);
        }

        // ─── Add Inventory ───
        [HttpPost("inventory/add")]
        public async Task<IActionResult> AddInventory([FromBody] AddInventoryDto dto)
        {
            var result = await _pharmacyService.AddInventoryAsync(dto);
            return Ok(result);
        }
        // ─── Get By Category ───
        [HttpGet("ByCategory/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var result = await _pharmacyService.GetByCategoryAsync(category);

            if (!result.Any())
                return NotFound("مفيش صيدليات عندها أدوية في الكاتيجوري دي!");

            return Ok(result);
        }

        // ─── Update Medicine ───
        [HttpPut("UpdateMedicine/{id}")]
        public async Task<IActionResult> UpdateMedicine(string id, [FromQuery] decimal price, [FromQuery] string productName, [FromQuery] string? category)
        {
            var result = await _pharmacyService.UpdateMedicineAsync(id, price, productName, category);

            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تعديل الدواء بنجاح!");
        }

        // ─── Update Stock ───
        [HttpPatch("UpdateStock/{id}/{newQuantity}")]
        public async Task<IActionResult> UpdateStock(string id, int newQuantity)
        {
            var result = await _pharmacyService.UpdateStockAsync(id, newQuantity);

            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تحديث الكمية بنجاح!");
        }

        // ─── Delete Medicine ───
        [HttpDelete("DeleteMedicine/{id}")]
        public async Task<IActionResult> DeleteMedicine(string id)
        {
            var result = await _pharmacyService.DeleteMedicineAsync(id);

            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم حذف الدواء بنجاح!");
        }
    }
}