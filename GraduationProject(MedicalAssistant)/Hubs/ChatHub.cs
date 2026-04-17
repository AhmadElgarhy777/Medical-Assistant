
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;



namespace GraduationProject_MedicalAssistant_.Hubs
{
    public class ChatHub:Hub
    {
    //    private readonly IConversationPaticipantsRepositry conversationPaticipantsRepositry;
    //    private readonly IMessageRepositry messageRepositry;

    //    public ChatHub(IConversationPaticipantsRepositry conversationPaticipantsRepositry,IMessageRepositry messageRepositry)
    //    {
    //        this.conversationPaticipantsRepositry = conversationPaticipantsRepositry;
    //        this.messageRepositry = messageRepositry;
    //    }
    //    public async Task JoinConversation(string conversationId)
    //    {
    //        var userId = Context.UserIdentifier;
    //        var spec =new ConversationParticiPantsSpecifcation(conversationId,userId);

    //        var participants =await conversationPaticipantsRepositry.GetAll(spec).ToListAsync();

    //        if (!participants.Any())
    //        {
    //             throw new Exception("Unauthorized");
    //        }

    //        await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");

    //    }
        
    //    public async Task SendMessage(string conversationId,string message)
    //    {
    //        var userId = Context.UserIdentifier;
    //        var spec =new ConversationParticiPantsSpecifcation(conversationId,userId);

    //        var participants =await conversationPaticipantsRepositry.GetAll(spec).ToListAsync();

    //        if (!participants.Any())
    //        {
    //             throw new Exception("Unauthorized");
    //        }

    //        var newMessage = new Messages()
    //        {
    //            ConversationId= conversationId,
    //            SenderId=userId,
    //            Content = message
    //        };
    //        messageRepositry.Add(newMessage);
    //        await messageRepositry.CommitAsync();

    //        await Clients.Group($"conversation-{conversationId}").SendAsync("ReceiveMessage", newMessage);
    //    }

    //    public override async Task OnConnectedAsync()
    //    {
    //        var userId = Context.UserIdentifier;
    //        await Clients.All.SendAsync("UserOnline", userId);

    //        await base.OnConnectedAsync();
    //    }

    //    public override async Task OnDisconnectedAsync(Exception? exception)
    //    {
    //        var userId = Context.UserIdentifier;
    //        await Clients.All.SendAsync("UserOffline", userId);

    //        await base.OnDisconnectedAsync(exception);
    //    }

    //    public async Task MarkConversationAsRead(string conversationId)
    //    {
    //        var userId = Context.UserIdentifier;

    //        var spec = new MessageSpecifcation(conversationId, userId, false);
    //        var messages = await messageRepositry.GetAll(spec).ToListAsync();
    //        foreach (var msg in messages)
    //        {
    //            msg.IsRead = true;
    //        }

    //        await messageRepositry.CommitAsync();

    //        await Clients.Group(conversationId.ToString())
    //            .SendAsync("ConversationRead", conversationId);
    //    }
    //    public async Task Typing(int conversationId)
    //    {
    //        var userId = Context.UserIdentifier;

    //        await Clients.OthersInGroup(conversationId.ToString())
    //            .SendAsync("UserTyping", userId);
    //    }
    }
}
