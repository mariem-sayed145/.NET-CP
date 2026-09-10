using ECommerce.Application.Chat.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Hubs;

public sealed class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(
        int conversationId,
        int senderCustomerId,
        string message)
    {
        var result = await _mediator.Send(
            new SendMessageCommand(
                conversationId,
                senderCustomerId,
                message));

        await Clients.Group($"conversation-{conversationId}")
            .SendAsync("ReceiveMessage", result);
    }

    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"conversation-{conversationId}");
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            $"conversation-{conversationId}");
    }
}