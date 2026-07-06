using DataAccess.Repositry.IRepositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.FileServices;
using System.Security.Claims;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public ChatController(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IFileService fileService,
            IConfiguration configuration)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _fileService = fileService;
            _configuration = configuration;
        }

        [HttpPost("conversation/{userId2}")]
        public async Task<IActionResult> CreateConversation(string userId2)
        {
            var userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var conversation = await _conversationRepository.CreateConversationAsync(userId1!, userId2);
            return Ok(new { conversationId = conversation.ID });
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _conversationRepository.GetUserConversationsAsync(userId!);
            if (!result.Any())
                return NotFound("مفيش محادثات!");
            return Ok(result);
        }

        [HttpGet("conversation/{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(string conversationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant)
                return Forbid();

            var messages = await _messageRepository.GetConversationMessagesAsync(conversationId);
            return Ok(messages);
        }

        [HttpPost("conversation/{conversationId}/image")]
        public async Task<IActionResult> SendImage(string conversationId, IFormFile image)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant) return Forbid();

            var imgName = await _fileService.UploadFileAsync(image, "ChatImages", CancellationToken.None);
            var imgUrl = $"{_configuration["ApiBaseUrL"]}/ChatImages/{imgName}";

            var message = new Models.Messages
            {
                ConversationId = conversationId,
                SenderId = userId!,
                MediaUrl = imgUrl,
                MediaType = "Image",
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
            return Ok(new { mediaUrl = imgUrl, messageId = message.ID });
        }

        [HttpPost("conversation/{conversationId}/file")]
        public async Task<IActionResult> SendFile(string conversationId, IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant) return Forbid();

            var fileName = await _fileService.UploadFileAsync(file, "ChatFiles", CancellationToken.None);
            var fileUrl = $"{_configuration["ApiBaseUrL"]}/ChatFiles/{fileName}";

            var message = new Models.Messages
            {
                ConversationId = conversationId,
                SenderId = userId!,
                MediaUrl = fileUrl,
                MediaType = "File",
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
            return Ok(new { mediaUrl = fileUrl, messageId = message.ID });
        }

        [HttpPost("conversation/{conversationId}/voice")]
        public async Task<IActionResult> SendVoice(string conversationId, IFormFile voice)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant) return Forbid();

            var voiceName = await _fileService.UploadFileAsync(voice, "ChatVoices", CancellationToken.None);
            var voiceUrl = $"{_configuration["ApiBaseUrL"]}/ChatVoices/{voiceName}";

            var message = new Models.Messages
            {
                ConversationId = conversationId,
                SenderId = userId!,
                MediaUrl = voiceUrl,
                MediaType = "Voice",
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
            return Ok(new { mediaUrl = voiceUrl, messageId = message.ID });
        }
        // ✅ حذف رسالة
        [HttpDelete("message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _messageRepository.DeleteMessageAsync(messageId, userId!);
            if (!result)
                return NotFound("الرسالة مش موجودة أو مش بتاعتك!");
            return Ok("تم حذف الرسالة!");
        }

        // ✅ تعديل رسالة
        [HttpPut("message/{messageId}")]
        public async Task<IActionResult> EditMessage(string messageId, [FromQuery] string newContent)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _messageRepository.EditMessageAsync(messageId, userId!, newContent);
            if (!result)
                return NotFound("الرسالة مش موجودة أو مش بتاعتك!");
            return Ok("تم تعديل الرسالة!");
        }

        // ✅ البحث في الرسايل
        [HttpGet("conversation/{conversationId}/search")]
        public async Task<IActionResult> SearchMessages(string conversationId, [FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("ادخل كلمة البحث!");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant)
                return Forbid();

            var result = await _messageRepository.SearchMessagesAsync(conversationId, query);
            if (!result.Any())
                return NotFound("مفيش رسايل بالكلمة دي!");
            return Ok(result);
        }
    }
}