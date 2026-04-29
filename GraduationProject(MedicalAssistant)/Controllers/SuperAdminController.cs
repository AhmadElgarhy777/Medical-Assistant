using AutoMapper;
using Features.PharmacyFeature;
using Features.SuperAdminFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =SD.SuperAdminRole)]
    public class SuperAdminController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public SuperAdminController(IPharmacyService pharmacyService,IMediator mediator,IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPut("ChangeSuperAdmin")]
        public async Task<IActionResult> ChangeSuperAdmin([FromBody] UpdateSuperAdminCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
        //// ✅ كل الصيدليات المنتظرة
        //[HttpGet("pharmacies/pending")]
        //public async Task<IActionResult> GetPendingPharmacies()
        //{
        //    var result = await _pharmacyService.GetPendingPharmaciesAsync();
        //    if (!result.Any())
        //        return NotFound("مفيش صيدليات منتظرة!");
        //    return Ok(result);
        //}

        // ✅ كل الصيدليات
        [HttpGet("GetAllPharmacies")]
        public async Task<IActionResult> GetAllPharmacies()
        {
            var result = await _pharmacyService.GetAllPharmaciesAsync();
            ///////////////////////////////////////////////
            if (!result.Any())
                return NotFound("مفيش صيدليات!");

            var resultDto = mapper.Map<IEnumerable<Pharmacy>, List<PharmaciesDTO>>(result);
            return Ok(resultDto);
        }

        //// ✅ الصيدليات المقبولة
        //[HttpGet("pharmacies/approved")]
        //public async Task<IActionResult> GetApprovedPharmacies()
        //{
        //    var result = await _pharmacyService.GetApprovedPharmaciesAsync();
        //    if (!result.Any())
        //        return NotFound("مفيش صيدليات مقبولة!");
        //    return Ok(result);
        //}

        //// ✅ الصيدليات المرفوضة
        //[HttpGet("pharmacies/rejected")]
        //public async Task<IActionResult> GetRejectedPharmacies()
        //{
        //    var result = await _pharmacyService.GetRejectedPharmaciesAsync();
        //    if (!result.Any())
        //        return NotFound("مفيش صيدليات مرفوضة!");
        //    return Ok(result);
        //}

        //// ✅ موافقة على صيدلية
        //[HttpPatch("pharmacy/{pharmacyId}/approve")]
        //public async Task<IActionResult> ApprovePharmacy(string pharmacyId)
        //{
        //    var result = await _pharmacyService.ApprovePharmacyAsync(pharmacyId);
        //    if (!result)
        //        return NotFound("الصيدلية مش موجودة!");
        //    return Ok("تم قبول الصيدلية!");
        //}

        //// ✅ رفض صيدلية
        //[HttpPatch("pharmacy/{pharmacyId}/reject")]
        //public async Task<IActionResult> RejectPharmacy(string pharmacyId)
        //{
        //    var result = await _pharmacyService.RejectPharmacyAsync(pharmacyId);
        //    if (!result)
        //        return NotFound("الصيدلية مش موجودة!");
        //    return Ok("تم رفض الصيدلية!");
        //}

        // ✅ حذف صيدلية
        [HttpDelete("DeletePharmacy")]
        public async Task<IActionResult> DeletePharmacy(string pharmacyId)
        {
            var result = await _pharmacyService.DeletePharmacyAsync(pharmacyId);
            if (!result)
                return NotFound("الصيدلية مش موجودة!");
            return Ok("تم حذف الصيدلية!");
        }
        // ✅ كل المرضى
        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _pharmacyService.GetAllPatientsAsync(); //////////////////////////////////////
            if (!result.Any())
                return NotFound("مفيش مرضى!");
            var resultDto = mapper.Map<IEnumerable<Patient>, List<PhatientsDTO>>(result);
            return Ok(resultDto);
        }

        // ✅ حذف مريض
        [HttpDelete("DeletePatient")]
        public async Task<IActionResult> DeletePatient(string patientId)
        {
            var result = await _pharmacyService.DeletePatientAsync(patientId);
            if (!result)
                return NotFound("المريض مش موجود!");
            return Ok("تم حذف المريض!");
        }

        // ✅ حظر مريض
        [HttpPatch("BanPatient")]
        public async Task<IActionResult> BanPatient(string patientId)
        {
            var result = await _pharmacyService.BanPatientAsync(patientId);
            if (!result)
                return NotFound("المريض مش موجود!");
            return Ok("تم حظر المريض!");
        }
        // ✅ كل الأطباء
        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _pharmacyService.GetAllDoctorsAsync(); /////////////////////////////////////
            if (!result.Any())
                return NotFound("مفيش أطباء!");
            var resultDto = mapper.Map<IEnumerable<Doctor>, List<DoctorsDTO>>(result);
            return Ok(resultDto);
        }

        // ✅ حذف دكتور
        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            var result = await _pharmacyService.DeleteDoctorAsync(doctorId);
            if (!result)
                return NotFound("الدكتور مش موجود!");
            return Ok("تم حذف الدكتور!");
        }

        // ✅ حظر دكتور
        [HttpPatch("BanDoctor")]
        public async Task<IActionResult> BanDoctor(string doctorId)
        {
            var result = await _pharmacyService.BanDoctorAsync(doctorId);
            if (!result)
                return NotFound("الدكتور مش موجود!");
            return Ok("تم حظر الدكتور!");
        }

        // ✅ إحصائيات السيستم
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _pharmacyService.GetStatsAsync();
            return Ok(result);
        }

        //// ✅ كل الطلبات
        //[HttpGet("orders")]
        //public async Task<IActionResult> GetAllOrders()
        //{
        //    var result = await _pharmacyService.GetAllOrdersAsync(); ////////////////////////////
        //    if (!result.Any())
        //        return NotFound("مفيش طلبات!");

        //    return Ok(result);
        //}
    }
}