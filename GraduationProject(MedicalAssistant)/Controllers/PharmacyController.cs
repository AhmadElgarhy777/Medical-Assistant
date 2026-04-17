using Features.PharmacyFeature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Enums;
using System.Security.Claims;

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



        // ✅ الأدمن بس يضيف صيدلية
        //[HttpPost("add")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddPharmacy([FromBody] AddPharmacyDto dto)
        //{
        //    var result = await _pharmacyService.AddPharmacyAsync(dto);
        //    return Ok(result);
        //}

        [HttpGet("SearchInSpecificPharmacy")]
        [AllowAnonymous]
        public async Task<List<DrugDTO>> SearchInSpecificPharmacy(string pharmacyId, string DrugNameOrCategory)
        {
            var Result=await _pharmacyService.SearchInSpecificPharmacyAsync(pharmacyId, DrugNameOrCategory);
            return Result.ToList();
        }

        // ✅ الصيدلية بس تضيف دواء
        [HttpPost("product/add")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            var pharmacyId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _pharmacyService.AddProductAsync(pharmacyId,dto);
            var productResult=new PharmcyProductDTO
            {
                Name=result.Name,
                Description=result.Description,
                Category=result.Category,
            };
            return Ok(productResult);
        }

        // ✅ الصيدلية بس تضيف Inventory
        [HttpPost("inventory/add")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> AddInventory([FromBody] AddInventoryDto dto)
        {
            var pharmacyId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _pharmacyService.AddInventoryAsync(pharmacyId ,dto);
            var InventoryResult = new InvetroyResultDTO
            {
                Price = result.Price,
                Quantity = result.Quantity,
                IsAvailable = result.IsAvailable
            };
            return Ok(InventoryResult);
        }

        [HttpGet("GetAllPharmacyInvetoryMedicine")]
        [Authorize(Roles = "Pharmacy")]

        public ActionResult<List<MedicineInvetoryListDTO>> GetAllPharmacyInvetoryMedicine()
        {
            var pharmacyId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _pharmacyService.GetAllPharmacyInvetoryMedicine(pharmacyId);
            return result;


        }
        // ✅ الصيدلية بس تعدل دواء
        [HttpPut("UpdateMedicine/{id}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateMedicine(string Invetoryid, [FromQuery] decimal price,
            [FromQuery] string productName, [FromQuery] string? category)
        {
            var result = await _pharmacyService.UpdateMedicineAsync(Invetoryid, price, productName, category);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تعديل الدواء بنجاح!");
        }

        // ✅ الصيدلية بس تحدث الكمية
        [HttpPatch("UpdateStock/{id}/{newQuantity}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateStock(string Invetoryid, int newQuantity)
        {
            var result = await _pharmacyService.UpdateStockAsync(Invetoryid, newQuantity);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم تحديث الكمية بنجاح!");
        }

        // ✅ الصيدلية بس تحذف دواء
        [HttpDelete("DeleteMedicine/{id}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> DeleteMedicine(string Invetoryid)
        {
            var result = await _pharmacyService.DeleteMedicineAsync(Invetoryid);
            if (!result)
                return NotFound("الدواء مش موجود!");

            return Ok("تم حذف الدواء بنجاح!");
        }

        // ✅ الصيدلية تغير حالتها
        //[HttpPatch("status/{pharmacyId}")]
        //[Authorize(Roles = "Pharmacy")]
        //public async Task<IActionResult> UpdatePharmacyStatus(string pharmacyId, [FromQuery] ConfrmationStatus status)
        //{
        //    await _pharmacyService.UpdatePharmacyStatusAsync(pharmacyId, status);
        //    return Ok($"تم تغيير حالة الصيدلية لـ {status}");
        //}



        // ✅ الصيدلية تشوف الأدوية على وشك النفاد
        //[HttpGet("lowstock/{pharmacyId}")]
        //[Authorize(Roles = "Pharmacy")]
        //public async Task<IActionResult> GetLowStock(string pharmacyId, [FromQuery] int threshold = 10)
        //{
        //    var result = await _pharmacyService.GetLowStockAsync(pharmacyId, threshold);

        //    if (!result.Any())
        //        return NotFound("مفيش أدوية على وشك النفاد!");

        //    return Ok(result);
        //}


        // ✅ جيب بيانات الصيدلية
        [HttpGet("{pharmacyId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> GetPharmacyById(string pharmacyId)
        {
            var result = await _pharmacyService.GetPharmacyByIdAsync(pharmacyId);
            if (result == null)
                return NotFound("الصيدلية مش موجودة!");

            return Ok(result);
        }

        // ✅ تعديل بيانات الصيدلية
        [HttpPut("update/{pharmacyId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdatePharmacyInfo(
            string pharmacyId,
            [FromQuery] string name,
            [FromQuery] string address,
            [FromQuery] string phone,
            [FromQuery] string city,
            [FromQuery] string governorate)
        {
            var result = await _pharmacyService.UpdatePharmacyInfoAsync(pharmacyId, name, address, phone, city, governorate);
            if (!result)
                return NotFound("الصيدلية مش موجودة!");

            return Ok("تم تعديل بيانات الصيدلية بنجاح!");

        }


        // Dashboard
        [HttpGet("dashboard/{pharmacyId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> GetDashboard(string pharmacyId)
        {
            var result = await _pharmacyService.GetDashboardAsync(pharmacyId);
            return Ok(result);
        }

        // إضافة تقييم
       

        // عرض متوسط التقييم
        [HttpGet("rating/{pharmacyId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAverageRating(string pharmacyId)
        {
            var result = await _pharmacyService.GetAverageRatingAsync(pharmacyId);
            return Ok(new { averageRating = result });
        }

        //[HttpGet("inventory/{pharmacyId}")]
        //[Authorize(Roles = "Pharmacy")]
        //public async Task<IActionResult> GetPharmacyInventory(string pharmacyId)
        //{
        //    var result = await _pharmacyService.GetPharmacyInventoryAsync(pharmacyId);
        //    return Ok(result);
        //}
    }
}