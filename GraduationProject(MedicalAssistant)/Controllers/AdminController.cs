using Features;
using Features.AdminFeature.Commands;
using Features.AdminFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Enums;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class AdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetPendingDoctors")]
        [ProducesResponseType(typeof(DoctorRowDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<List<DoctorRowDTO>>> GetPendingDoctors(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var Result = await mediator.Send(new GetPendingDoctorsQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles =$"{SD.AdminRole},{SD.SuperAdminRole},{SD.PatientRole}")]
        [HttpGet("GetDoctorDetails")]
        [ProducesResponseType(typeof(DoctorDetailsDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<DoctorDetailsDTO>> GetDoctorDetails([FromQuery]string doctorId,CancellationToken cancellationToken)
        {
            var Result = await mediator.Send(new GetDoctorDetailsQuery(doctorId,cancellationToken));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }
        
        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetConfirmedDoctors")]
        [ProducesResponseType(typeof(DoctorRowDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DoctorRowDTO>>> GetConfirmedDoctors(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var Result = await mediator.Send(new GetConfirmedDoctorsQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }


        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpPut("ChangeStatus")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<String>>> ChangeStatus([FromQuery] ChangeStatusCommand command , CancellationToken cancellationToken)
        {
            var Result=await mediator.Send(command,cancellationToken);
            if (Result.ISucsses)
            {
                return Ok(Result.Message);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetPendingNurse")]
        [ProducesResponseType(typeof(NurseRowDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NurseRowDTO>>> GetPendingNurse(CancellationToken cancellationToken, [FromQuery] int page = 1)
        {
            var Result = await mediator.Send(new GetPendingNurseQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetNurseDetails")]
        [ProducesResponseType(typeof(NurseDetailseDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<NurseDetailseDTO>> GetNurseDetails([FromQuery] string nurseId, CancellationToken cancellationToken)
        {
            var Result = await mediator.Send(new GetNurseDetailsQuery(nurseId, cancellationToken));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetConfirmedNurse")]
        [ProducesResponseType(typeof(NurseRowDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<List<NurseRowDTO>>> GetConfirmedNurse(CancellationToken cancellationToken, [FromQuery] int page = 1)
        {
            var Result = await mediator.Send(new GetConfirmedNurseQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetPharmacyByStatus")]
        [ProducesResponseType(typeof(PharmacyRowDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<List<PharmacyRowDTO>>> GetPharmacyByStatus(CancellationToken cancellationToken,ConfrmationStatus status, [FromQuery] int page = 1)
        {
            var Result = await mediator.Send(new GetPharmcyByStatusQuery(cancellationToken, status, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [Authorize(Roles = $"{SD.AdminRole},{SD.SuperAdminRole}")]
        [HttpGet("GetPharmacyDetails")]
        [ProducesResponseType(typeof(PharmacyDetailsDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<PharmacyDetailsDTO>> GetPharmacyDetails([FromQuery] string pharmacyId, CancellationToken cancellationToken)
        {
            var Result = await mediator.Send(new GetPharmcyDetailsQuery(pharmacyId, cancellationToken));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

    }
}
