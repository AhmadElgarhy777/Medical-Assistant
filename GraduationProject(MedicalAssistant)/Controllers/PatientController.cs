using Features.PatientFeature.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator mediatR;

        public PatientController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [HttpGet("GetDoctorsBySearch")]
        public async Task<IActionResult> GetDoctorsBySearch([FromQuery] GetAllDoctorsSearchQuery query ,CancellationToken cancellationToken)
        {
                var doctors = await mediatR.Send(query, cancellationToken);
                if (doctors.Any())
                {
                    return Ok(doctors);
                }
                return NotFound();
            
        }
    }
}
