using Features;
using Features.AppointmentFeature.Commands;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Enums;
using System.Security.Claims;
using Utility;
using static System.Net.WebRequestMethods;

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
        [ProducesResponseType(typeof(ResultResponse<String>), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.PatientRole}")]

        public async Task<ActionResult<ResultResponse<String>>> Book([FromBody] CreateAppointmentCommand command)
        {
            // نصيحة: دايماً اتأكد إن الـ command مش فاضي قبل ما تبعته
            if (command == null || string.IsNullOrEmpty(command.SlotId))
            {
                return BadRequest("بيانات الحجز غير مكتملة، يرجى اختيار ميعاد صحيح.");
            }

            var result = await _mediatR.Send(command);

            if (result.ISucsses)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("AvailableSlots")]
        [ProducesResponseType(typeof(DoctorAvailableTimeDTO), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.PatientRole},{SD.AdminRole}")]
        public async Task<ActionResult> GetAvailableSlots([FromQuery]string doctorId)
        {
            var result = await _mediatR.Send(new GetDoctorAvailableSlotsQuery(doctorId));
            if (result.Any())
            {
                return Ok(result);
            }
            return BadRequest("No Time are Available");
        }

        //[HttpPut("Cancel/{id}")]
        //[ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        //[Authorize(Roles = $"{SD.DoctorRole}")]
        //public async Task<IActionResult> Cancel(string id)
        //{
        //    var result = await _mediatR.Send(new CancelAppointmentCommand(id));
        //    if (!result) return BadRequest("تعذر إلغاء الحجز، ربما الموعد غير موجود");
        //    return Ok("تم إلغاء الحجز بنجاح، والموعد متاح الآن لمريض آخر");
        //}


        [HttpPut("UpdateStatus")]
        [ProducesResponseType(typeof(ResultResponse<String>), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.DoctorRole}")]
        public async Task<ActionResult<ResultResponse<String>>> UpdateStatus([FromQuery] string AppointmentId, bookStatusEnum NewStatus)
        {
            var docID =HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediatR.Send(new UpdateAppointmentStatusCommand(AppointmentId,NewStatus,docID));

            if (result.ISucsses)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
