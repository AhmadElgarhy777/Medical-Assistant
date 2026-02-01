using Features;
using Features.DoctorFeature.Commands;
using Features.DoctorFeature.Queries;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using System.Security.Claims;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Authorize(Roles =$"{SD.DoctorRole},{SD.AdminRole}")]
    public class DoctorController : ApiBaseController
    {
        private readonly IMediator _mediatR;

        public DoctorController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }



        [HttpGet("Profile")]
        [ProducesResponseType(typeof(DoctorDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<DoctorDTO>>> Profile(CancellationToken cancellationToken)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var doctor = await _mediatR.Send(new GetDoctorProfileQuery(doctorId, cancellationToken));
            if (doctor.ISucsses)
            {
                return Ok(doctor.Obj);
            }
            return NotFound();
        }

        // Endpoint عشان الدكتور يجيب قائمة المرضى بتوعه
        [HttpGet("MyPatients")]
        [ProducesResponseType(typeof(PatientDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult> GetMyPatients()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetDoctorPatientsQuery(doctorId);
            var result = await _mediatR.Send(query);

            if (result != null && result.Any())
            {
                return Ok(result);
            }

            return NotFound("لا يوجد مرضى مسجلين لهذا الدكتور حتى الآن");
        }


        [HttpPost("AddAvailability")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAvailability(DayOfWeek Day, string FromTime,string ToTime)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new CreateDoctorSlotsCommand(doctorId,Day,FromTime,ToTime));
            return Ok(result);
        }


        [HttpGet("AvailableSlots")]
        [ProducesResponseType(typeof(DoctorAvailableTimeDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorAvailableSlotsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("MyAppointments")]
        [ProducesResponseType(typeof(DoctorAppointmentsDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetMyAppointments()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorAppointmentsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("history")]
        [ProducesResponseType(typeof(DoctorAppointmentsDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetHistory()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorHistoryQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("stats")]
        [ProducesResponseType(typeof(DoctorStatsDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetDoctorStats()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var stats = await _mediatR.Send(new GetDoctorStatsQuery(doctorId));
            return Ok(stats);
        }


        [HttpPost("CreatePrescription")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]

        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionCommand command)
        {
            var result = await _mediatR.Send(command);
            if (result)
            {
                return Ok(new { message = "الروشتة اتسجلت بنجاح والمريض يقدر يشوفها دلوقتي" });
            }
            return BadRequest("حصل مشكلة وأحنا بنسجل الروشتة");
        }
    }
}
