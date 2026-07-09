using AutoMapper;
using Features.PharmacyFeature;
using Features.SuperAdminFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Utility;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Features;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class SuperAdminController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminController(IPharmacyService pharmacyService, IMediator mediator, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _pharmacyService = pharmacyService;
            this.mediator = mediator;
            this.mapper = mapper;
            _userManager = userManager;
        }

        [HttpPut("ChangeSuperAdmin")]
        public async Task<IActionResult> ChangeSuperAdmin([FromBody] UpdateSuperAdminCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

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

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _pharmacyService.GetAllDoctorsAsync(); /////////////////////////////////////
            if (!result.Any())
                return NotFound("مفيش أطباء!");
            var resultDto = mapper.Map<IEnumerable<Doctor>, List<DoctorsDTO>>(result);
            return Ok(resultDto);
        }


        [HttpPut("Ban&&DeletePharmacy")]
        public async Task<IActionResult> DeletePharmacy(string pharmacyId)
        {
            var result = await mediator.Send(new DeletePharmacyCommand(pharmacyId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }

        // ✅ حذف مريض
        [HttpPut("Ban&&DeletePatient")]
        public async Task<IActionResult> DeletePatient(string patientId)
        {
            var result = await mediator.Send(new DeletePatientCommand(patientId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }


        // ✅ حذف دكتور
        [HttpPut("Ban&&DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            var result = await mediator.Send(new DeleteDoctorCommand(doctorId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }
        [HttpPut("Ban&&DeleteNurse")]
        public async Task<IActionResult> DeleteNurse(string nurseId)
        {
            var result = await mediator.Send(new DeleteNurseCommand(nurseId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }

        [HttpPut("UnBanPatient")]
        public async Task<IActionResult> UnBanPatient(string patientId)
        {
            var result = await mediator.Send(new UnbanPatientCommand(patientId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }
        [HttpPut("UnBanDoctor")]
        public async Task<IActionResult> UnBanDoctor(string doctorId)
        {
            var result = await mediator.Send(new UnbanDoctorCommand(doctorId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }
        [HttpPut("UnBanPharmacy")]
        public async Task<IActionResult> UnBanPharmacy(string pharmacyId)
        {
            var result = await mediator.Send(new UnbanPhramacyCommand(pharmacyId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }
        [HttpPut("UnBanNurse")]
        public async Task<IActionResult> UnBanNurse(string nurseId)
        {
            var result = await mediator.Send(new UnBanNurseCommand(nurseId));
            if (!result.ISucsses)
                return BadRequest(result);
            return Ok(result.Obj);
        }

        [HttpPost("BanUser")]
        public async Task<IActionResult> BanUser([FromBody] BanUserRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound("المستخدم غير موجود");

            var roles = await _userManager.GetRolesAsync(user);
            string userRole = roles.FirstOrDefault() ?? "Unknown";

            ResultResponse<bool> result = null;

            if (userRole == SD.DoctorRole)
                result = await mediator.Send(new DeleteDoctorCommand(request.UserId));
            else if (userRole == SD.PatientRole)
                result = await mediator.Send(new DeletePatientCommand(request.UserId));
            else if (userRole == SD.PharmacyRole)
                result = await mediator.Send(new DeletePharmacyCommand(request.UserId));
            else if (userRole == SD.NurseRole)
                result = await mediator.Send(new DeleteNurseCommand(request.UserId));

            if (result != null && !result.ISucsses)
                return BadRequest(result.Message);

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminName = User.FindFirstValue(ClaimTypes.Name) ?? "Admin";

            var report = new BanReport
            {
                BannedUserId = user.Id,
                BannedUserName = user.UserName ?? "Unknown",
                BannedUserEmail = user.Email ?? "Unknown",
                AdminId = adminId ?? "",
                AdminName = adminName,
                Reason = request.Reason,
                UserType = userRole,
                BanDate = DateTime.UtcNow,
                BanCount = 1
            };

            await _pharmacyService.AddBanReportAsync(report);

            return Ok(new { message = "تم حظر المستخدم بنجاح" });
        }

        [HttpPost("UnbanUser")]
        public async Task<IActionResult> UnbanUser([FromBody] UnbanUserRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound("المستخدم غير موجود");

            var roles = await _userManager.GetRolesAsync(user);
            string userRole = roles.FirstOrDefault() ?? "Unknown";

            ResultResponse<bool> result = null;

            if (userRole == SD.DoctorRole)
                result = await mediator.Send(new UnbanDoctorCommand(request.UserId));
            else if (userRole == SD.PatientRole)
                result = await mediator.Send(new UnbanPatientCommand(request.UserId));
            else if (userRole == SD.PharmacyRole)
                result = await mediator.Send(new UnbanPhramacyCommand(request.UserId));
            else if (userRole == SD.NurseRole)
                result = await mediator.Send(new UnBanNurseCommand(request.UserId));

            if (result != null && !result.ISucsses)
                return BadRequest(result.Message);

            return Ok(new { message = "تم إلغاء الحظر عن المستخدم بنجاح" });
        }        // ✅ إضافة Ban Report
        [HttpPost("ban-report")]
        public async Task<IActionResult> AddBanReport([FromBody] BanReport banReport)
        {
            await _pharmacyService.AddBanReportAsync(banReport);
            return Ok("تم إضافة تقرير الحظر!");
        }

        // ✅ كل تقارير الحظر
        [HttpGet("ban-reports")]
        public async Task<IActionResult> GetAllBanReports()
        {
            var result = await _pharmacyService.GetAllBanReportsAsync();
            if (!result.Any())
                return NotFound("مفيش تقارير حظر!");
            return Ok(result);
        }

        // ✅ تقرير حظر واحد
        [HttpGet("ban-report/{id}")]
        public async Task<IActionResult> GetBanReportById(string id)
        {
            var result = await _pharmacyService.GetBanReportByIdAsync(id);
            if (result == null)
                return NotFound("التقرير مش موجود!");
            return Ok(result);
        }

        // ✅ تقارير حظر يوزر معين
        [HttpGet("ban-reports/user/{userId}")]
        public async Task<IActionResult> GetUserBanReports(string userId)
        {
            var result = await _pharmacyService.GetUserBanReportsAsync(userId);
            if (!result.Any())
                return NotFound("مفيش تقارير لهذا المستخدم!");
            return Ok(result);
        }

        // ✅ إضافة شكوى
        [HttpPost("complaint")]
        [AllowAnonymous]
        public async Task<IActionResult> AddComplaint([FromBody] Complaint complaint)
        {
            complaint.CreatedAt = DateTime.Now;
            await _pharmacyService.AddComplaintAsync(complaint);
            return Ok("تم إرسال الشكوى بنجاح!");
        }

        // ✅ كل الشكاوي
        [HttpGet("complaints")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var result = await _pharmacyService.GetAllComplaintsAsync();
            if (!result.Any())
                return NotFound("مفيش شكاوي!");
            return Ok(result);
        }

        // ✅ تحديد شكوى كمقروءة
        [HttpPatch("complaint/{complaintId}/read")]
        public async Task<IActionResult> MarkComplaintAsRead(string complaintId)
        {
            var result = await _pharmacyService.MarkComplaintAsReadAsync(complaintId);
            if (!result)
                return NotFound("الشكوى مش موجودة!");
            return Ok("تم تحديد الشكوى كمقروءة!");
        }

        // ✅ بحث شامل
        [HttpGet("search")]
        public async Task<IActionResult> SearchAll([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("ادخل كلمة البحث!");
            var result = await _pharmacyService.SearchAllAsync(query);
            if (!result.Any())
                return NotFound("مفيش نتايج!");
            return Ok(result);
        }

        // ✅ كل التقييمات
        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings()
        {
            var result = await _pharmacyService.GetAllRatingsAsync();
            if (!result.Any())
                return NotFound("مفيش تقييمات!");
            return Ok(result);
        }

        // ✅ حذف تقييم
        [HttpDelete("rating/{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            var result = await _pharmacyService.DeleteRatingAsync(ratingId);
            if (!result)
                return NotFound("التقييم مش موجود!");
            return Ok("تم حذف التقييم!");
        }


        // ✅ إحصائيات السيستم
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _pharmacyService.GetStatsAsync();
            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetSuperAdminDashboard()
        {
            var result = await _pharmacyService.GetSuperAdminDashboardAsync();
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