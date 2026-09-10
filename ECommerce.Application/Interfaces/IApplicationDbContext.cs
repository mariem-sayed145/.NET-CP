using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ChatConversation> ChatConversations { get; }

    DbSet<ChatMessage> ChatMessages { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}