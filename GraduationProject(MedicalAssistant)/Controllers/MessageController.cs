using DataAccess.Repositry.IRepositry;
using Features;
using Features.MessagesFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace GraduationProject_MedicalAssistant_.Controllers
{
   
    public class MessageController : ApiBaseController
    {
        private readonly IMediator mediator;

        public MessageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetConversationMessages")]
        [ProducesResponseType(typeof(List<Messages>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultResponse<Messages>),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultResponse<List<Messages>>>> GetConversationMessages([FromQuery]string conversationId,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetConversationMessagesQuery(conversationId, cancellationToken));
            if (result.ISucsses)
            {
                return Ok(result.Obj);
            }
            return NotFound(result.Message);

        }
    }
}
