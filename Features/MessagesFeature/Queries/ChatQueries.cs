using Features;
using MediatR;
using Models.DTOs;
using System.Collections.Generic;

namespace Features.MessagesFeature.Queries
{
    public class GetConversationsQuery : IRequest<ResultResponse<List<ConversationDto>>>
    {
    }

    // Overriding the existing GetConversationMessagesQuery to use pagination and DTOs
    public class GetPaginatedMessagesQuery : IRequest<ResultResponse<object>>
    {
        public string ConversationId { get; set; } = null!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class SearchMessagesQuery : IRequest<ResultResponse<object>>
    {
        public string ConversationId { get; set; } = null!;
        public string Query { get; set; } = null!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetOrCreateConversationQuery : IRequest<ResultResponse<string>>
    {
        public string UserId2 { get; set; } = null!;
    }
}
