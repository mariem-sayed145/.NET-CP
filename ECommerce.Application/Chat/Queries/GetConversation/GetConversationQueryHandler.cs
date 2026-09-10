using ECommerce.Application.Chat.DTOs;
using ECommerce.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Chat.Queries.GetConversation;

public sealed class GetConversationQueryHandler
    : IRequestHandler<GetConversationQuery, IReadOnlyList<ChatMessageResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetConversationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ChatMessageResponse>> Handle(
        GetConversationQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.ChatMessages
            .AsNoTracking()
            .Where(x => x.ConversationId == request.ConversationId)
            .OrderBy(x => x.SentAt)
            .Select(x => new ChatMessageResponse(
                x.Id,
                x.ConversationId,
                x.SenderCustomerId,
                x.Message,
                x.SentAt))
            .ToListAsync(cancellationToken);
    }
}