using Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using System.Collections.Generic;

namespace Features.MessagesFeature.Commands
{
    public class SendMessageCommand : IRequest<ResultResponse<MessageDto>>
    {
        public string ConversationId { get; set; } = null!;
        public string? Content { get; set; }
        public IFormFile? MediaFile { get; set; }
        public string MediaType { get; set; } = "Text"; // Text, Image, File, Voice
    }

    public class EditMessageCommand : IRequest<ResultResponse<string>>
    {
        public string MessageId { get; set; } = null!;
        public string NewContent { get; set; } = null!;
    }

    public class DeleteMessageCommand : IRequest<ResultResponse<string>>
    {
        public string MessageId { get; set; } = null!;
    }
}
