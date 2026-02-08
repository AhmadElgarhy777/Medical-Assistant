using Features;
using Features.AdminFeature.Commands;
using Features.AdminFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Enums;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class AdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetPendingDoctors")]
        public async Task<ActionResult<List<DoctorRowDTO>>> GetPendingDoctors(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var Result = await mediator.Send(new GetPendingDoctorsQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }
        
        [HttpGet("GetDoctorDetails")]
        public async Task<ActionResult<DoctorDetailsDTO>> GetDoctorDetails([FromQuery]string doctorId,CancellationToken cancellationToken)
        {
            var Result = await mediator.Send(new GetDoctorDetailsQuery(doctorId,cancellationToken));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }
        
        
        [HttpGet("GetConfirmedDoctors")]
        public async Task<ActionResult<List<DoctorRowDTO>>> GetConfirmedDoctors(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var Result = await mediator.Send(new GetConfirmedDoctorsQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }
        
        [HttpPut("ChangeStatus")]
        public async Task<ActionResult<ResultResponse<String>>> ChangeStatus([FromQuery] ChangeStatusCommand command , CancellationToken cancellationToken)
        {
            var Result=await mediator.Send(command,cancellationToken);
            if (Result.ISucsses)
            {
                return Ok(Result.Message);
            }
            return BadRequest(Result.Message);
        }

        [HttpGet("GetPendingNurse")]
        public async Task<ActionResult<List<DoctorRowDTO>>> GetPendingNurse(CancellationToken cancellationToken, [FromQuery] int page = 1)
        {
            var Result = await mediator.Send(new GetPendingNurseQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

        [HttpGet("GetNurseDetails")]
        public async Task<ActionResult<DoctorDetailsDTO>> GetNurseDetails([FromQuery] string nurseId, CancellationToken cancellationToken)
        {
            var Result = await mediator.Send(new GetNurseDetailsQuery(nurseId, cancellationToken));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }


        [HttpGet("GetConfirmedNurse")]
        public async Task<ActionResult<List<DoctorRowDTO>>> GetConfirmedNurse(CancellationToken cancellationToken, [FromQuery] int page = 1)
        {
            var Result = await mediator.Send(new GetConfirmedNurseQuery(cancellationToken, page));
            if (Result.ISucsses)
            {
                return Ok(Result.Obj);
            }
            return BadRequest(Result.Message);
        }

    }
}
