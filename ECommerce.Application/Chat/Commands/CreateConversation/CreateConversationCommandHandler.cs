using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Chat.Commands.CreateConversation;

public sealed class CreateConversationCommandHandler
    : IRequestHandler<CreateConversationCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var customerExists = await _context.Customers
            .AnyAsync(
                x => x.Id == request.CustomerId,
                cancellationToken);

        if (!customerExists)
            throw new InvalidOperationException("Customer not found.");

        var existingConversation = await _context.ChatConversations
            .FirstOrDefaultAsync(
                x => x.CustomerId == request.CustomerId,
                cancellationToken);

        if (existingConversation is not null)
            return existingConversation.Id;

        var conversation = new ChatConversation(request.CustomerId);

        _context.ChatConversations.Add(conversation);

        await _context.SaveChangesAsync(cancellationToken);

        return conversation.Id;
    }
}