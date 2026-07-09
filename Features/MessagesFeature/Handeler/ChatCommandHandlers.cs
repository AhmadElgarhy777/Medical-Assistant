using DataAccess.Repositry.IRepositry;
using Features.MessagesFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models.DTOs;
using Services.FileServices;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;
using FluentValidation;

namespace Features.MessagesFeature.Handeler
{
    public class ChatCommandHandlers :
        IRequestHandler<SendMessageCommand, ResultResponse<MessageDto>>,
        IRequestHandler<EditMessageCommand, ResultResponse<string>>,
        IRequestHandler<DeleteMessageCommand, ResultResponse<string>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<SendMessageCommand> _sendMessageValidator;
        private readonly IValidator<EditMessageCommand> _editMessageValidator;
        private readonly IValidator<DeleteMessageCommand> _deleteMessageValidator;

        public ChatCommandHandlers(
            IMessageRepository messageRepository,
            IConversationRepository conversationRepository,
            IFileService fileService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IValidator<SendMessageCommand> sendMessageValidator,
            IValidator<EditMessageCommand> editMessageValidator,
            IValidator<DeleteMessageCommand> deleteMessageValidator)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _fileService = fileService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _sendMessageValidator = sendMessageValidator;
            _editMessageValidator = editMessageValidator;
            _deleteMessageValidator = deleteMessageValidator;
        }

        private string? GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<MessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _sendMessageValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ResultResponse<MessageDto> { ISucsses = false, Message = error };
            }

            var userId = GetUserId();
            if (userId == null) return new ResultResponse<MessageDto> { ISucsses = false, Message = "Unauthorized" };

            var isParticipant = await _conversationRepository.IsParticipantAsync(request.ConversationId, userId);
            if (!isParticipant) return new ResultResponse<MessageDto> { ISucsses = false, Message = "Not a participant" };

            string? mediaUrl = null;
            if (request.MediaFile != null)
            {
                string folder = request.MediaType switch
                {
                    "Image" => "ChatImages",
                    "Voice" => "ChatVoices",
                    "File" => "ChatFiles",
                    _ => "ChatFiles"
                };
                var fileName = await _fileService.UploadFileAsync(request.MediaFile, folder, cancellationToken);
                mediaUrl = $"{_configuration["ApiBaseUrL"]}/{folder}/{fileName}";
            }

            var message = new Models.Messages
            {
                ConversationId = request.ConversationId,
                SenderId = userId,
                Content = request.Content,
                MediaUrl = mediaUrl,
                MediaType = request.MediaType,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
            
            var user = await _userManager.FindByIdAsync(userId);

            var dto = new MessageDto
            {
                MessageId = message.ID,
                SenderId = message.SenderId,
                SenderName = user?.UserName ?? "Unknown",
                Content = message.Content,
                MediaUrl = message.MediaUrl,
                MediaType = message.MediaType,
                IsRead = false,
                SentAt = message.SentAt,
                IsEdited = false,
                IsDeleted = false
            };

            return new ResultResponse<MessageDto> { ISucsses = true, Obj = dto };
        }

        public async Task<ResultResponse<string>> Handle(EditMessageCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _editMessageValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ResultResponse<string> { ISucsses = false, Message = error };
            }

            var userId = GetUserId();
            if (userId == null) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var result = await _messageRepository.EditMessageAsync(request.MessageId, userId, request.NewContent);
            if (!result) return new ResultResponse<string> { ISucsses = false, Message = "Message not found or not owned by you" };

            return new ResultResponse<string> { ISucsses = true, Message = "Message edited successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _deleteMessageValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ResultResponse<string> { ISucsses = false, Message = error };
            }

            var userId = GetUserId();
            if (userId == null) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var result = await _messageRepository.DeleteMessageAsync(request.MessageId, userId);
            if (!result) return new ResultResponse<string> { ISucsses = false, Message = "Message not found or not owned by you" };

            return new ResultResponse<string> { ISucsses = true, Message = "Message deleted successfully" };
        }
    }
}
