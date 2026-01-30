using Features.DoctorFeature.Commands;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("MyPatients/{doctorId}")]
        public async Task<IActionResult> GetMyPatients(string doctorId)
        {
            var query = new GetDoctorPatientsQuery(doctorId);
            var result = await _mediatR.Send(query);

            if (result != null && result.Any())
            {
                return Ok(result);
            }

            return NotFound("لا يوجد مرضى مسجلين لهذا الدكتور حتى الآن");
        }


        [HttpPost("AddAvailability")]
        public async Task<IActionResult> AddAvailability(CreateDoctorSlotsCommand command)
        {
            var result = await _mediatR.Send(command);
            return Ok(result);
        }


        [HttpGet("AvailableSlots/{doctorId}")]
        public async Task<IActionResult> GetAvailableSlots(string doctorId)
        {
            var result = await _mediatR.Send(new GetDoctorAvailableSlotsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("MyAppointments/{doctorId}")]
        public async Task<IActionResult> GetMyAppointments(string doctorId)
        {
            var result = await _mediatR.Send(new GetDoctorAppointmentsQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("{doctorId}/history")]
        public async Task<IActionResult> GetHistory(string doctorId)
        {
            var result = await _mediatR.Send(new GetDoctorHistoryQuery(doctorId));
            return Ok(result);
        }


        [HttpGet("{doctorId}/stats")]
        public async Task<IActionResult> GetDoctorStats(string doctorId)
        {
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
