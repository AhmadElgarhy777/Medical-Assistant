using Features;
using Features.PatientFeature.Queries;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Security.Claims;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    //[Authorize(Roles = $"{SD.AdminRole},{SD.PatientRole}")]
    public class PatientController : ApiBaseController
    {
        private readonly IMediator mediatR;

        public PatientController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [HttpGet("GetDoctorsBySearch")]
        [ProducesResponseType(typeof(DoctorDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<DoctorDTO>>>> GetDoctorsBySearch([FromQuery] GetAllDoctorsSearchQuery query ,CancellationToken cancellationToken)
        {
                var doctors = await mediatR.Send(query, cancellationToken);
                if (doctors is not null)
                {
                    return Ok(doctors.Obj);
                }
                return NotFound(doctors.Message);
            
        }
        [HttpGet("GetNurseBySearch")]
        [ProducesResponseType(typeof(NurseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<NurseDTO>> GetNurseBySearch([FromQuery] SearchNuresQuery query ,CancellationToken cancellationToken)
        {
                var nurses = await mediatR.Send(query, cancellationToken);
                if (nurses.Any())
                {
                    return Ok(nurses);
                }
                return NotFound();
            
        }


        [HttpGet("Profile")]
        [ProducesResponseType(typeof(PatientPeofileDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<PatientPeofileDTO>>> Profile(CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patient = await mediatR.Send(new GetPatientProfileQuery(patientId, cancellationToken)) ;
            if (patient.ISucsses)
            {
                return Ok(patient.Obj);
            }
            return NotFound();
        }

        [HttpGet("Appoinments")]
        [ProducesResponseType(typeof(AppointmentDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<AppointmentDTO>>>> Appoinments([FromQuery] int page,CancellationToken cancellationToken)
        {
           var patientId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appoinment=await mediatR.Send(new GetAppointmentsQuery(patientId, page), cancellationToken);
            if (appoinment.ISucsses)
            {
                return Ok(appoinment.Obj);
            }
            return NotFound(appoinment);
        }

        [HttpGet("Reports")]
        [ProducesResponseType(typeof(AiReportDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<AiReportDTO>>>> Reports([FromQuery] int page, CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Report = await mediatR.Send(new GetAiReportsQuery(patientId,page), cancellationToken);
            if (Report.ISucsses)
            {
                return Ok(Report.Obj);
            }
            return NotFound(Report);
        }
        
        [HttpGet("Presciptions")]
        [ProducesResponseType(typeof(PresciptionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<PresciptionDTO>>>> Presciptions([FromQuery] int page, CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var presciptions = await mediatR.Send(new GetPresciptionQuery(patientId,page), cancellationToken);
            if (presciptions.ISucsses)
            {
                return Ok(presciptions.Obj);
            }
            return NotFound(presciptions);
        }
        
        //[HttpGet("PresciptionItems")]
        //public async Task<ActionResult<PresciptionItemDTO>> PresciptionItems([FromQuery] GetPresciptionItemQuery query, CancellationToken cancellationToken)
        //{
        //    var Items = await mediatR.Send(query, cancellationToken);
        //    if (Items.Any())
        //    {
        //        return Ok(Items); 
        //    }
        //    return NotFound();
        //}


        [HttpGet("GetFullPrescriptionDetails/{id}")]
        [ProducesResponseType(typeof(PrescriptionFullDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetFullPrescriptionDetails(string id) // غيرنا النوع هنا لـ string
        {
            // دلوقتى الـ id نوعه string والـ Query مستنية string
            var result = await mediatR.Send(new GetFullPrescriptionQuery(id));

            return result != null ? Ok(result) : NotFound("الروشتة غير موجودة");
        }

    }
}
