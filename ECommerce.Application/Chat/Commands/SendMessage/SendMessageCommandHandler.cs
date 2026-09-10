using ECommerce.Application.Chat.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Chat.Commands.SendMessage;

public sealed class SendMessageCommandHandler
    : IRequestHandler<SendMessageCommand, ChatMessageResponse>
{
    private readonly IApplicationDbContext _context;

    public SendMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessageResponse> Handle(
        SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _context.ChatConversations
            .FirstOrDefaultAsync(
                x => x.Id == request.ConversationId,
                cancellationToken);

        if (conversation is null)
            throw new InvalidOperationException("Conversation not found.");

        var message = new ChatMessage(
            request.ConversationId,
            request.SenderCustomerId,
            request.Message);

        _context.ChatMessages.Add(message);

        conversation.UpdateLastMessageTime();

        await _context.SaveChangesAsync(cancellationToken);

        return new ChatMessageResponse(
            message.Id,
            message.ConversationId,
            message.SenderCustomerId,
            message.Message,
            message.SentAt);
    }
}