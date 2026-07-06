using Features;
using Features.LabFeature.Commands;
using Features.LabFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class LabController : ApiBaseController
    {
        private readonly IMediator _mediatR;
        public LabController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet("Areas")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetAreas([FromQuery] Models.Enums.Governorate governorate)
        {
            var result = await _mediatR.Send(new GetAreasByGovernorateQuery(governorate));
            return Ok(result);
        }

        [HttpGet("ByArea")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetLabsByArea([FromQuery] string areaId)
        {
            var result = await _mediatR.Send(new GetLabsByAreaQuery(areaId));
            return Ok(result);
        }

        [HttpGet("Details/{labId}")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetLabDetails(string labId, [FromQuery] string? search)
        {
            var result = await _mediatR.Send(new GetLabDetailsQuery(labId, search));
            if (result == null) return NotFound("المعمل غير موجود");
            return Ok(result);
        }

        [HttpGet("SearchTests")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> SearchTests([FromQuery] string? search)
        {
            var result = await _mediatR.Send(new SearchMedicalTestsQuery(search));
            return Ok(result);
        }

        [HttpPost("BookNow")]
        [ProducesResponseType(typeof(ResultResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<ActionResult<ResultResponse<string>>> Book([FromBody] CreateLabBookingCommand command)
        {
            if (command == null)
                return BadRequest("بيانات الحجز غير مكتملة");

            var result = await _mediatR.Send(command);
            if (result.ISucsses)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("MyBookings")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetMyBookings([FromQuery] string patientId)
        {
            var result = await _mediatR.Send(new GetPatientLabBookingsQuery(patientId));
            return Ok(result);
        }
        [HttpPost("AddTestOffer")]
        [Authorize(Roles = $"{SD.LabRole}")]
        [ProducesResponseType(typeof(ResultResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<string>>> AddTestOffer(
    [FromBody] AddLabTestOfferCommand command)
        {
            var result = await _mediatR.Send(command);
            if (result.ISucsses)
                return Ok(result);
            return BadRequest(result);
        }
    }
}