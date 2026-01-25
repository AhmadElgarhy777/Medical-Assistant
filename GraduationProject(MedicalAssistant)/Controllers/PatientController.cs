using Features.PatientFeature.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Controllers
{
  
    public class PatientController : ApiBaseController
    {
        private readonly IMediator mediatR;

        public PatientController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [HttpGet("GetDoctorsBySearch")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorsBySearch([FromQuery] GetAllDoctorsSearchQuery query ,CancellationToken cancellationToken)
        {
                var doctors = await mediatR.Send(query, cancellationToken);
                if (doctors.Any())
                {
                    return Ok(doctors);
                }
                return NotFound();
            
        }
        [HttpGet("GetNurseBySearch")]

        public async Task<ActionResult<NurseDTO>> GetNurseBySearch([FromQuery] SearchNuresQuery query ,CancellationToken cancellationToken)
        {
                var nurses = await mediatR.Send(query, cancellationToken);
                if (nurses.Any())
                {
                    return Ok(nurses);
                }
                return NotFound();
            
        }
        [HttpGet("Appoinments")]
        public async Task<ActionResult<AppointmentDTO>> Appoinments([FromQuery] GetAppointmentsQuery query,CancellationToken cancellationToken)
        {
            var appoinment=await mediatR.Send(query,cancellationToken);
            if (appoinment.Any())
            {
                return Ok(appoinment);
            }
            return NotFound();
        }

        [HttpGet("Reports")]
        public async Task<ActionResult<AiReportDTO>> Reports([FromQuery] GetAiReportsQuery query, CancellationToken cancellationToken)
        {
            var Report = await mediatR.Send(query, cancellationToken);
            if (Report.Any())
            {
                return Ok(Report);
            }
            return NotFound();
        }
        
        [HttpGet("Presciptions")]
        public async Task<ActionResult<PresciptionDTO>> Presciptions([FromQuery] GetPresciptionQuery query, CancellationToken cancellationToken)
        {
            var presciptions = await mediatR.Send(query, cancellationToken);
            if (presciptions.Any())
            {
                return Ok(presciptions);
            }
            return NotFound();
        }
        
        [HttpGet("PresciptionItems")]
        public async Task<ActionResult<PresciptionItemDTO>> PresciptionItems([FromQuery] GetPresciptionItemQuery query, CancellationToken cancellationToken)
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
