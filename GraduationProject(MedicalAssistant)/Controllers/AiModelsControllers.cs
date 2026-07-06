using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
using Features.AiFeature.CBCBloodTest;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SkinCancerClassification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utility;
using static System.Net.Mime.MediaTypeNames;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Authorize(Roles = $"{SD.DoctorRole}, {SD.AdminRole}, {SD.SuperAdminRole}")]
    public class AiModelsControllers : ApiBaseController
    {
        private readonly IMediator mediator;

        public AiModelsControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("brain-tumor")]
        public async Task<IActionResult> AnalyzeBrainTumor([FromForm] List<IFormFile> Images,[FromForm]string PatientId,[FromForm]string? DoctorNote)
        {
            var DoctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new AnalyzeBrainTumorCommand(Images, PatientId, DoctorId, DoctorNote));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("skin-cancer")]
        public async Task<IActionResult> AnalyzeSkinCancer([FromForm] List<IFormFile> Images, [FromForm] string PatientId, [FromForm] string? DoctorNote)
        {
            var DoctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await mediator.Send(new SkinCancerClassificationCommand(Images, PatientId, DoctorId, DoctorNote));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("chest-ray")]
        public async Task<IActionResult> AnalyzeChestRay([FromForm] List<IFormFile> Images, [FromForm] string PatientId, [FromForm] string? DoctorNote)
        {
            var DoctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await mediator.Send(new ChestRayClassifcationCommand(Images, PatientId, DoctorId, DoctorNote));
            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("cbc-blood-test")]
        public async Task<IActionResult> AnalyzeCBCBloodTest([FromForm] List<IFormFile> Images, [FromForm] string PatientId, [FromForm] string? DoctorNote)
        {
            var DoctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await mediator.Send(new CBCBloodTestCommand(Images, PatientId, DoctorId, DoctorNote));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
