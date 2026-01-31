using Features.DoctorFeature.Commands;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class DoctorController : ApiBaseController
    {
        private readonly IMediator _mediatR;

        public DoctorController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        // Endpoint عشان الدكتور يجيب قائمة المرضى بتوعه
        [HttpGet("MyPatients")]
        public async Task<IActionResult> GetMyPatients()
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
        public async Task<IActionResult> AddAvailability(DayOfWeek Day, string FromTime,string ToTime)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new CreateDoctorSlotsCommand(doctorId,Day,FromTime,ToTime));
            return Ok(result);
        }


        [HttpGet("AvailableSlots")]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorAvailableSlotsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("MyAppointments")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorAppointmentsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediatR.Send(new GetDoctorHistoryQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("stats")]
        public async Task<IActionResult> GetDoctorStats()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var stats = await _mediatR.Send(new GetDoctorStatsQuery(doctorId));
            return Ok(stats);
        }


        [HttpPost("CreatePrescription")]
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
