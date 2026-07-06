using Features;
using Features.RegisterationFeature.Commands;
using Features.RegisterationFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.RegistertionDTOs;

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
                    return Ok(result);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }

        [HttpPost("VerifyAdminInvitation")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> VerifyAdminInvitation([FromQuery] AccepAdmintInvitationCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
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
                    return Ok(result);
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
                    return Ok(result);
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
                    return Ok(result);
                }
                return BadRequest(result);

            }
            return BadRequest("Validation Error");

        }

        [HttpPost("RegisterPharmacy")]
        [ProducesResponseType(typeof(ResultResponse<String>), StatusCodes.Status200OK)]

        public async Task<ActionResult> RegisterPharmacy([FromForm] RegistrationPharmacyCommand command, CancellationToken cancellationToken)
        {

            var result = await mediator.Send(command, cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("ConfirmationEmailType")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> ConfirmationEmailType([FromQuery] ConfirmationEmailTypeCommand command,CancellationToken cancellationToken)
        {
            var result=await mediator.Send(command,cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
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
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
      
        [HttpPost("VerifyEmailOTP")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult<ResultResponse<String>>> VerifyOTP([FromQuery] VerifyOTPCommand command,CancellationToken cancellationToken)
        {
            var result=await mediator.Send(command,cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
       

        [HttpPost("ConfirmMobileNumberViaWhatsUp")]
        public async Task<IActionResult> ConfirmMobileNumberViaWhatsUp([FromForm] ConfirmMobileNumberCommand command)
        {
            var result = await mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [HttpPost("VerifyMobileNumberOtp")]
        public async Task<IActionResult> VerifyMobileNumberOtp([FromForm] VerifyMobileNumberOtpCommand command)
        {
            var result = await mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [HttpPost("RegisterLab")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<string>>> RegisterLab(
    [FromForm] RegisterLabDTO dto, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterLabCommand(dto, cancellationToken));
            if (result.ISucsses)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
