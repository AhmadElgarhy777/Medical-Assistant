using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccess.Repositry.IRepositry
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Messages message);
        Task<IEnumerable<Messages>> GetConversationMessagesAsync(string conversationId);
        Task MarkMessagesAsReadAsync(string conversationId, string userId);
        Task<bool> DeleteMessageAsync(string messageId, string userId);
        Task<bool> EditMessageAsync(string messageId, string userId, string newContent);
        Task<IEnumerable<Messages>> SearchMessagesAsync(string conversationId, string query);
    }
}

