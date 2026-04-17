using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.MessagesFeature.Queries
{
    public record GetConversationMessagesQuery(string conversationId,CancellationToken CancellationToken) :IRequest<ResultResponse<List<Messages>>>;
    
}
