using Features;
using Features.RegisterationFeature.Commands;
using Features.RegisterationFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    
    public class RegisterationController : ApiBaseController
    {
        private readonly IMediator mediator;
        public RegisterationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("RegisterAdmin")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> RegisterAdmin([FromForm]RegisterationAdminCommand command , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(command, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }


        [HttpPost("GetAllSpecialization")]
        [ProducesResponseType(typeof(SpecializationDTO), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<List<SpecializationDTO>>>> GetAllSpecialization([FromForm] GetAllSpecializationQuery query , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(query, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Obj);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }
        [HttpPost("RegisterDoctor")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> RegisterDoctor([FromForm] RegisterationDoctorCommand command , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(command, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }


        [HttpPost("RegisterNurse")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> RegisterNurse([FromForm] RegisterationNurseCommand command , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(command, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }
        
        [HttpPost("RegisterPatient")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> RegisterPatient([FromForm] RegisterationPatientCommand command , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(command, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }


        [HttpPost("ConfirmationEmailType")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> ConfirmationEmailType([FromQuery] ConfirmationEmailTypeCommand command,CancellationToken cancellationToken)
        {
            var result=await mediator.Send(command,cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        
        [HttpPost("VerifyEmail")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> VerifyEmail([FromQuery] VerifyEmailCommand command,CancellationToken cancellationToken)
        {
            var result=await mediator.Send(command,cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
      
        [HttpPost("VerifyOTP")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> VerifyOTP([FromQuery] VerifyOTPCommand command,CancellationToken cancellationToken)
        {
            var result=await mediator.Send(command,cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }



    }
}
