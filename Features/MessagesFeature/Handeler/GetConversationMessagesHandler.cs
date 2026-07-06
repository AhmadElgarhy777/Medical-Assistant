using DataAccess.Repositry.IRepositry;
using Features.MessagesFeature.Queries;
using MediatR;
using Models;

namespace Features.MessagesFeature.Handeler
{
    public class GetConversationMessagesHandler : IRequestHandler<GetConversationMessagesQuery, ResultResponse<List<Messages>>>
    {
        private readonly IMessageRepository messageRepositry;
        private readonly IConversationPaticipantsRepositry conversationPaticipantsRepositry;
        private readonly IConversationRepository conversationRepositry;

        public GetConversationMessagesHandler(
            IMessageRepository messageRepositry,
            IConversationPaticipantsRepositry conversationPaticipantsRepositry,
            IConversationRepository conversationRepositry)
        {
            this.messageRepositry = messageRepositry;
            this.conversationPaticipantsRepositry = conversationPaticipantsRepositry;
            this.conversationRepositry = conversationRepositry;
        }

        public async Task<ResultResponse<List<Messages>>> Handle(GetConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await messageRepositry.GetConversationMessagesAsync(request.conversationId);

            if (messages.Any())
            {
                return new ResultResponse<List<Messages>>
                {
                    ISucsses = true,
                    Obj = messages.ToList(),
                };
            }
            return new ResultResponse<List<Messages>>
            {
                ISucsses = false,
                Message = "No Messages Yet"
            };
        }
    }
}