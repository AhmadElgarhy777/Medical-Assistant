using Features.NotifecationService.Commands.MarkAsRead;
using Features.NotifecationService.Queries.GetNotifcation;
using Features.NotifecationService.Queries.GetUnreadCount;
using Features.PatientScanRequet.Commands.AnalyzeRequestedScanOrcastraor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Authorize]
    public class NotifcationController : ApiBaseController
    {
        private readonly IMediator mediator;

        public NotifcationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("GetMyNotifcation")]
        public async Task<IActionResult> GetMyNotifcation(
    CancellationToken cancellationToken)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(
                new GetNotificationsQuery(
                    userID
                    ), cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result.Obj);
        }
       
        
        [HttpPost("GetUnreadCount")]
        public async Task<IActionResult> GetUnreadCount(
    CancellationToken cancellationToken)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(
                new GetUnreadCountQuery(
                    userID
                    ), cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result.Obj);
        }
      
        
        
        
        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead([FromQuery]string NotificationId,
    CancellationToken cancellationToken)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(
                new MarkAsReadCommand(
                    NotificationId,userID
                    ), cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result.Obj);
        }
    }
}
