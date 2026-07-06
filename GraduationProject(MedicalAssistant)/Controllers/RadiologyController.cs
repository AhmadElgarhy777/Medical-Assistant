using Features;
using Features.RadiologyFeature.Commands;
using Features.RadiologyFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class RadiologyController : ApiBaseController
    {
        private readonly IMediator _mediatR;
        public RadiologyController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet("ByArea")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetCentersByArea([FromQuery] string areaId)
        {
            var result = await _mediatR.Send(new GetRadiologyCentersByAreaQuery(areaId));
            return Ok(result);
        }

        [HttpGet("Details/{centerId}")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> GetCenterDetails(string centerId, [FromQuery] string? search)
        {
            var result = await _mediatR.Send(new GetRadiologyCenterDetailsQuery(centerId, search));
            if (result == null) return NotFound("المركز غير موجود");
            return Ok(result);
        }

        [HttpPost("BookNow")]
        [ProducesResponseType(typeof(ResultResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<ActionResult<ResultResponse<string>>> Book([FromBody] CreateRadiologyBookingCommand command)
        {
            if (command == null)
                return BadRequest("بيانات الحجز غير مكتملة");

            var result = await _mediatR.Send(command);
            if (result.ISucsses)
                return Ok(result);
            return BadRequest(result);
        }
    }
}