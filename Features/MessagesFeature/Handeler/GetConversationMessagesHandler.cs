using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.MessagesFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.MessagesFeature.Handeler
{
    public class GetConversationMessagesHandler : IRequestHandler<GetConversationMessagesQuery, ResultResponse<List<Messages>>>
    {
        private readonly IMessageRepositry messageRepositry;
        private readonly IConversationPaticipantsRepositry conversationPaticipantsRepositry;
        private readonly IConversationRepositry conversationRepositry;

        public GetConversationMessagesHandler(IMessageRepositry messageRepositry,
            IConversationPaticipantsRepositry conversationPaticipantsRepositry,
            IConversationRepositry conversationRepositry)
        {
            this.messageRepositry = messageRepositry;
            this.conversationPaticipantsRepositry = conversationPaticipantsRepositry;
            this.conversationRepositry = conversationRepositry;
        }
        public async Task<ResultResponse<List<Messages>>> Handle(GetConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var spec = new MessageSpecifcation(request.conversationId);
            var messages = await messageRepositry.GetAll(spec)
                .OrderBy(m=>m.SentAt)
                .ToListAsync(cancellationToken);

            if (messages.Any())
            {
                return new ResultResponse<List<Messages>>
                {
                    ISucsses = true,
                    Obj = messages,

                };
            }


            return new ResultResponse<List<Messages>>
            {
                ISucsses = false,
                Message="No Messages Yet "

            };



        }
    }
}
