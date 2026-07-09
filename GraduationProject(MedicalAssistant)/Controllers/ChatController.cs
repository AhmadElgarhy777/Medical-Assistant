using Features.MessagesFeature.Commands;
using Features.MessagesFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private IActionResult HandleResult<T>(Features.ResultResponse<T> result)
        {
            if (result.ISucsses)
            {
                if (result.Obj != null) return Ok(result.Obj);
                return Ok(new { message = result.Message });
            }

            if (result.Message == "Unauthorized") return Unauthorized(new { message = result.Message });
            if (result.Message == "Not a participant") return StatusCode(403, new { message = result.Message });
            if (result.Message == "Message not found or not owned by you") return NotFound(new { message = result.Message });

            return BadRequest(new { message = result.Message ?? "Bad Request" });
        }

        [HttpPost("conversation/{userId2}")]
        public async Task<IActionResult> CreateConversation(string userId2)
        {
            var result = await _mediator.Send(new GetOrCreateConversationQuery { UserId2 = userId2 });
            if (!result.ISucsses) return HandleResult(result);
            return Ok(new { conversationId = result.Obj });
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetMyConversations()
        {
            var result = await _mediator.Send(new GetConversationsQuery());
            if (!result.ISucsses) return HandleResult(result);
            if (result.Obj == null || result.Obj.Count == 0) return NotFound(new { message = "مفيش محادثات!" });
            return Ok(result.Obj);
        }

        [HttpGet("conversation/{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(string conversationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetPaginatedMessagesQuery
            {
                ConversationId = conversationId,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return HandleResult(result);
        }

        [HttpPost("conversation/{conversationId}/message")]
        public async Task<IActionResult> SendTextMessage(string conversationId, [FromBody] SendTextRequestDto request)
        {
            var result = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = conversationId,
                Content = request.Text,
                MediaType = "Text"
            });
            return HandleResult(result);
        }

        [HttpPost("conversation/{conversationId}/image")]
        public async Task<IActionResult> SendImage(string conversationId, IFormFile image, [FromForm] string? caption)
        {
            var result = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = conversationId,
                MediaFile = image,
                Content = caption, // user allowed "عنوان الملف"
                MediaType = "Image"
            });
            return HandleResult(result);
        }

        [HttpPost("conversation/{conversationId}/file")]
        public async Task<IActionResult> SendFile(string conversationId, IFormFile file, [FromForm] string? caption)
        {
            var result = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = conversationId,
                MediaFile = file,
                Content = caption, // user allowed "عنوان الملف"
                MediaType = "File"
            });
            return HandleResult(result);
        }

        [HttpPost("conversation/{conversationId}/voice")]
        public async Task<IActionResult> SendVoice(string conversationId, IFormFile voice, [FromForm] string? duration)
        {
            var result = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = conversationId,
                MediaFile = voice,
                // duration is postponed until DB schema is modified, not saved in Content
                Content = null,
                MediaType = "Voice"
            });
            return HandleResult(result);
        }

        [HttpDelete("message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var result = await _mediator.Send(new DeleteMessageCommand { MessageId = messageId });
            return HandleResult(result);
        }

        [HttpPut("message/{messageId}")]
        public async Task<IActionResult> EditMessage(string messageId, [FromQuery] string newContent)
        {
            var result = await _mediator.Send(new EditMessageCommand { MessageId = messageId, NewContent = newContent });
            return HandleResult(result);
        }

        [HttpGet("conversation/{conversationId}/search")]
        public async Task<IActionResult> SearchMessages(string conversationId, [FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrEmpty(query)) return BadRequest(new { message = "ادخل كلمة البحث!" });

            var result = await _mediator.Send(new SearchMessagesQuery
            {
                ConversationId = conversationId,
                Query = query,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if (!result.ISucsses) return HandleResult(result);

            return Ok(result.Obj);
        }
    }
}