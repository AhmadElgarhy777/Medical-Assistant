using DataAccess.Repositry.IRepositry;
using Features.MessagesFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Features.MessagesFeature.Handeler
{
    public class ChatQueryHandlers :
        IRequestHandler<GetConversationsQuery, ResultResponse<List<ConversationDto>>>,
        IRequestHandler<GetPaginatedMessagesQuery, ResultResponse<object>>,
        IRequestHandler<SearchMessagesQuery, ResultResponse<object>>,
        IRequestHandler<GetOrCreateConversationQuery, ResultResponse<string>>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatQueryHandlers(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string? GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<List<ConversationDto>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null) return new ResultResponse<List<ConversationDto>> { ISucsses = false, Message = "Unauthorized" };

            var conversations = await _conversationRepository.GetUserConversationsAsync(userId);
            var dtos = new List<ConversationDto>();

            var participantIds = conversations
                .SelectMany(c => c.conversationParticipants)
                .Where(p => p.UserId != userId)
                .Select(p => p.UserId)
                .Distinct()
                .ToList();

            // Improve N+1: Batch fetch all other participants at once
            var usersList = _userManager.Users.Where(u => participantIds.Contains(u.Id)).ToList();
            
            // Note: Fetching Roles still requires individual queries via GetRolesAsync per user.
            // A complete fix for N+1 roles requires joining AspNetUserRoles in DbContext, which breaks Repository pattern.
            var userRolesDict = new Dictionary<string, string>();
            foreach (var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesDict[user.Id] = roles.FirstOrDefault() ?? "Unknown";
            }

            foreach (var c in conversations)
            {
                var otherParticipant = c.conversationParticipants.FirstOrDefault(p => p.UserId != userId);
                var otherPartyName = "Unknown";
                var otherPartyType = "Unknown";
                string? otherPartyImage = null;

                if (otherParticipant != null)
                {
                    var user = usersList.FirstOrDefault(u => u.Id == otherParticipant.UserId);
                    if (user != null)
                    {
                        otherPartyName = user.UserName ?? "Unknown";
                        otherPartyType = userRolesDict.ContainsKey(user.Id) ? userRolesDict[user.Id] : "Unknown";
                    }
                }

                var lastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault(m => !m.IsDeleted);
                
                // Very basic unread count (for true count we need a count query)
                var unreadCount = c.Messages.Count(m => m.SenderId != userId && !m.IsRead && !m.IsDeleted);

                dtos.Add(new ConversationDto
                {
                    ConversationId = c.ID,
                    OtherPartyName = otherPartyName,
                    OtherPartyImage = otherPartyImage,
                    OtherPartyType = otherPartyType,
                    LastMessage = lastMessage?.Content ?? (lastMessage?.MediaUrl != null ? "Media Attachment" : null),
                    LastMessageTime = lastMessage?.SentAt,
                    UnreadCount = unreadCount
                });
            }

            return new ResultResponse<List<ConversationDto>>
            {
                ISucsses = true,
                Obj = dtos.OrderByDescending(d => d.LastMessageTime).ToList()
            };
        }

        public async Task<ResultResponse<object>> Handle(GetPaginatedMessagesQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null) return new ResultResponse<object> { ISucsses = false, Message = "Unauthorized" };

            var isParticipant = await _conversationRepository.IsParticipantAsync(request.ConversationId, userId);
            if (!isParticipant) return new ResultResponse<object> { ISucsses = false, Message = "Not a participant" };

            var allMessages = await _messageRepository.GetConversationMessagesAsync(request.ConversationId);
            
            // Mark as read
            await _messageRepository.MarkMessagesAsReadAsync(request.ConversationId, userId);

            var pagedMessages = allMessages
                .OrderByDescending(m => m.SentAt) // Newest first
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = new List<MessageDto>();
            foreach (var m in pagedMessages)
            {
                var user = await _userManager.FindByIdAsync(m.SenderId);
                dtos.Add(new MessageDto
                {
                    MessageId = m.ID,
                    SenderId = m.SenderId,
                    SenderName = user?.UserName ?? "Unknown",
                    Content = m.Content,
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType,
                    IsRead = m.IsRead,
                    SentAt = m.SentAt,
                    IsEdited = false, // DB Modification needed to support IsEdited accurately
                    IsDeleted = m.IsDeleted
                });
            }

            var result = new
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = allMessages.Count(),
                Messages = dtos.OrderBy(d => d.SentAt).ToList() // Return chronological order
            };

            return new ResultResponse<object> { ISucsses = true, Obj = result };
        }

        public async Task<ResultResponse<object>> Handle(SearchMessagesQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null) return new ResultResponse<object> { ISucsses = false, Message = "Unauthorized" };

            var isParticipant = await _conversationRepository.IsParticipantAsync(request.ConversationId, userId);
            if (!isParticipant) return new ResultResponse<object> { ISucsses = false, Message = "Not a participant" };

            var searchResults = await _messageRepository.SearchMessagesAsync(request.ConversationId, request.Query);

            var pagedResults = searchResults
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = new List<MessageDto>();
            foreach (var m in pagedResults)
            {
                var user = await _userManager.FindByIdAsync(m.SenderId);
                dtos.Add(new MessageDto
                {
                    MessageId = m.ID,
                    SenderId = m.SenderId,
                    SenderName = user?.UserName ?? "Unknown",
                    Content = m.Content,
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType,
                    IsRead = m.IsRead,
                    SentAt = m.SentAt,
                    IsEdited = false, // DB Modification needed to support IsEdited accurately
                    IsDeleted = m.IsDeleted
                });
            }

            var result = new
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = searchResults.Count(),
                Messages = dtos
            };

            return new ResultResponse<object> { ISucsses = true, Obj = result };
        }

        public async Task<ResultResponse<string>> Handle(GetOrCreateConversationQuery request, CancellationToken cancellationToken)
        {
            var userId1 = GetUserId();
            if (userId1 == null) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var conversation = await _conversationRepository.CreateConversationAsync(userId1, request.UserId2);
            return new ResultResponse<string> { ISucsses = true, Obj = conversation.ID };
        }
    }
}
