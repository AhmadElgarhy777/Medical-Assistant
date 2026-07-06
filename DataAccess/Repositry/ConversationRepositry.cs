using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.EntityFrameworkCore;

using Models.Enums;

namespace DataAccess.Repositry.IRepositry
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync(string userId1, string userId2)
        {
            var conversation = new Conversation
            {
                Type = ConversationType.Private
            };

            await _context.Conversations.AddAsync(conversation);

            var participant1 = new ConversationParticipant
            {
                ConversationId = conversation.ID,
                UserId = userId1
            };

            var participant2 = new ConversationParticipant
            {
                ConversationId = conversation.ID,
                UserId = userId2
            };

            await _context.conversationParticipants.AddAsync(participant1);
            await _context.conversationParticipants.AddAsync(participant2);
            await _context.SaveChangesAsync();

            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .Where(c => c.conversationParticipants.Any(p => p.UserId == userId))
                .Include(c => c.conversationParticipants)
                .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
                .ToListAsync();
        }

        public async Task<bool> IsParticipantAsync(string conversationId, string userId)
        {
            return await _context.conversationParticipants
                .AnyAsync(p => p.ConversationId == conversationId && p.UserId == userId);
        }

        public async Task<Conversation> GetConversationByIdAsync(string conversationId)
        {
            return await _context.Conversations
                .Include(c => c.conversationParticipants)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ID == conversationId);
        }
    }
}
