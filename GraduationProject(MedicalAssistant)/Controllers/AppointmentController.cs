using Features.AppointmentFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    // بنورث من ApiBaseController زي ما أنت عامل في المريض
    public class AppointmentController : ApiBaseController
    {
        private readonly IMediator _mediatR;

        public AppointmentController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("BookNow")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        [Authorize(Roles = $"{SD.PatientRole},{SD.AdminRole}")]

        public async Task<IActionResult> Book([FromBody] CreateAppointmentCommand command)
        {
            // نصيحة: دايماً اتأكد إن الـ command مش فاضي قبل ما تبعته
            if (command == null || string.IsNullOrEmpty(command.SlotId))
            {
                return BadRequest("بيانات الحجز غير مكتملة، يرجى اختيار ميعاد صحيح.");
            }

            var result = await _mediatR.Send(command);

            if (result)
            {
                return Ok(new { message = "تم الحجز بنجاح، وتمت إضافة المريض لقائمة الدكتور" });
            }

            return BadRequest("الموعد ربما تم حجزه بالفعل من قبل مريض آخر");
        }
        [HttpPut("Cancel/{id}")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.DoctorRole},{SD.AdminRole}")]
        public async Task<IActionResult> Cancel(string id)
        {
            var result = await _mediatR.Send(new CancelAppointmentCommand(id));
            if (!result) return BadRequest("تعذر إلغاء الحجز، ربما الموعد غير موجود");
            return Ok("تم إلغاء الحجز بنجاح، والموعد متاح الآن لمريض آخر");
        }
        [HttpPut("UpdateStatus")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.DoctorRole},{SD.AdminRole}")]

        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAppointmentStatusCommand command)
        {
            var result = await _mediatR.Send(command);

            if (result)
                return Ok(new { message = "تم تحديث حالة الموعد بنجاح" });

            return BadRequest("فشل تحديث الحالة، تأكد من صحة رقم الحجز");
        }
    }
}
