using DataAccess.Repositry.IRepositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace GraduationProject_MedicalAssistant_.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;

        public ChatHub(
            IMessageRepository messageRepository,
            IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
        }

        public async Task JoinConversation(string conversationId)
        {
            var userId = Context.UserIdentifier;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant)
                throw new Exception("Unauthorized");
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");
        }

        public async Task SendMessage(string conversationId, string content)
        {
            var userId = Context.UserIdentifier;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant)
                throw new Exception("Unauthorized");

            var message = new Messages
            {
                ConversationId = conversationId,
                SenderId = userId!,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageRepository.AddMessageAsync(message);

            await Clients.Group($"conversation-{conversationId}").SendAsync("ReceiveMessage", new
            {
                messageId = message.ID,
                senderId = userId,
                content = content,
                mediaUrl = (string?)null,
                mediaType = (string?)null,
                sentAt = message.SentAt
            });
        }

        // ✅ بعت Media
        public async Task SendMedia(string conversationId, string mediaUrl, string mediaType)
        {
            var userId = Context.UserIdentifier;
            var isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId!);
            if (!isParticipant)
                throw new Exception("Unauthorized");

            await Clients.Group($"conversation-{conversationId}").SendAsync("ReceiveMessage", new
            {
                senderId = userId,
                content = (string?)null,
                mediaUrl = mediaUrl,
                mediaType = mediaType,
                sentAt = DateTime.UtcNow
            });
        }

        public async Task MarkAsRead(string conversationId)
        {
            var userId = Context.UserIdentifier;
            await _messageRepository.MarkMessagesAsReadAsync(conversationId, userId!);
            await Clients.Group($"conversation-{conversationId}")
                .SendAsync("ConversationRead", conversationId);
        }

        public async Task Typing(string conversationId)
        {
            var userId = Context.UserIdentifier;
            await Clients.OthersInGroup($"conversation-{conversationId}")
                .SendAsync("UserTyping", userId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            await Clients.All.SendAsync("UserOnline", userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            await Clients.All.SendAsync("UserOffline", userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}