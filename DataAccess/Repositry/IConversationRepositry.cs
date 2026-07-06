using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Models;

namespace DataAccess.Repositry.IRepositry
{
    public interface IConversationRepository
    {
        Task<Conversation> CreateConversationAsync(string userId1, string userId2);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
        Task<bool> IsParticipantAsync(string conversationId, string userId);
        Task<Conversation> GetConversationByIdAsync(string conversationId);
    }
}