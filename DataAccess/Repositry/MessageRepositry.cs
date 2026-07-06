using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repositry.IRepositry
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Messages message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Messages>> GetConversationMessagesAsync(string conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task MarkMessagesAsReadAsync(string conversationId, string userId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId &&
                       m.SenderId != userId &&
                       !m.IsRead)
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteMessageAsync(string messageId, string userId)
        {
            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.ID == messageId && m.SenderId == userId);
            if (message == null) return false;
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditMessageAsync(string messageId, string userId, string newContent)
        {
            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.ID == messageId && m.SenderId == userId);
            if (message == null) return false;
            message.Content = newContent;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Messages>> SearchMessagesAsync(string conversationId, string query)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId &&
                       m.Content != null &&
                       m.Content.Contains(query))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}