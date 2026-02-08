using Features;
using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Quieries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    
    public class AuthenticationController : ApiBaseController
    {
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor http;

        public AuthenticationController(IMediator mediator,IHttpContextAccessor http)
        {
            this.mediator = mediator;
            this.http = http;
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult<AuthDTO>> LogIn([FromForm]LogInUserCommand command ,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsSuucessed)
            {
                return Ok(result);
            }
            return Unauthorized(result.Message);
        }
       
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthDTO>> RefreshToken([FromForm] string RefreshToken ,CancellationToken cancellationToken)
        {
            var IpAddress = http.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var DeviceInfo = http.HttpContext?.Request?.Headers?["User-Agent"].ToString();

            var result = await mediator.Send(new RefreshTokenCommand(RefreshToken,IpAddress,DeviceInfo, cancellationToken));

            if (result.IsSuucessed)
            {
                return Ok(result);
            }
            return Unauthorized(result.Message);
        }

        [HttpPost("LogOut")]
        public async Task<ActionResult<bool>> LogOut([FromForm] string RefreshToken ,CancellationToken cancellationToken)
        {
            var IpAddress = http.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var DeviceInfo = http.HttpContext?.Request?.Headers?["User-Agent"].ToString();

            var result = await mediator.Send(new LogOutCommand(RefreshToken,IpAddress,DeviceInfo,cancellationToken));

            if (result)
            {
                return Ok("LogedOut Succesully");
            }
            return BadRequest("Invalid Token or already revoked");
        }


        [HttpPut("ChangePasssword")]
        [Authorize]
        public async Task<ActionResult<ResultResponse<String>>> ChangePassword([FromForm] ChangePasswordCommand command,CancellationToken cancellationToken)
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
        
        [HttpPost("ForgetPasssword")]
        public async Task<ActionResult<ResultResponse<String>>> ForgetPasssword([FromForm] ForgetPassswordCommand command,CancellationToken cancellationToken)
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

        [HttpPut("ResetPassword")]
        public async Task<ActionResult<ResultResponse<String>>> ResetPassword([FromForm] ResetPassswordCommand command, CancellationToken cancellationToken)
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
        
        [HttpPut("UpdateProfile")]
        public async Task<ActionResult<ResultResponse<UpdateProfileDTO>>> UpdateProfile([FromForm] UpdateProfileCommand command, CancellationToken cancellationToken)
        {
           
                var result = await mediator.Send(command, cancellationToken);
                if (result.ISucsses)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result);
            

        }

    }
}
