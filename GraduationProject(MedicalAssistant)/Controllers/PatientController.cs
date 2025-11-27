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
        [HttpGet("GetNurseBySearch")]

        public async Task<IActionResult> GetNurseBySearch([FromQuery] SearchNuresQuery query ,CancellationToken cancellationToken)
        {
                var nurses = await mediatR.Send(query, cancellationToken);
                if (nurses.Any())
                {
                    return Ok(nurses);
                }
                return NotFound();
            
        }
        [HttpGet("Appoinments")]
        public async Task<IActionResult> Appoinments([FromQuery] GetAppointmentsQuery query,CancellationToken cancellationToken)
        {
            var appoinment=await mediatR.Send(query,cancellationToken);
            if (appoinment.Any())
            {
                return Ok(appoinment);
            }
            return NotFound();
        }

        [HttpGet("Reports")]
        public async Task<IActionResult> Reports([FromQuery] GetAiReportsQuery query, CancellationToken cancellationToken)
        {
            var Report = await mediatR.Send(query, cancellationToken);
            if (Report.Any())
            {
                return Ok(Report);
            }
            return NotFound();
        }
        
        [HttpGet("Presciptions")]
        public async Task<IActionResult> Presciptions([FromQuery] GetPresciptionQuery query, CancellationToken cancellationToken)
        {
            var presciptions = await mediatR.Send(query, cancellationToken);
            if (presciptions.Any())
            {
                return Ok(presciptions);
            }
            return NotFound();
        }
        
        [HttpGet("PresciptionItems")]
        public async Task<IActionResult> PresciptionItems([FromQuery] GetPresciptionItemQuery query, CancellationToken cancellationToken)
        {
            var Items = await mediatR.Send(query, cancellationToken);
            if (Items.Any())
            {
                return Ok(Items);
            }
            return NotFound();
        }


    }
}
