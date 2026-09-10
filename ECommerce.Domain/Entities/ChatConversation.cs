using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public sealed class ChatConversation : Entity
{
    public int CustomerId { get; private set; }

    public Customer? Customer { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? LastMessageAt { get; private set; }

    private readonly List<ChatMessage> _messages = new();

    public IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();

    private ChatConversation()
    {
    }

    public ChatConversation(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("Invalid Customer ID.");

        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateLastMessageTime()
    {
        LastMessageAt = DateTime.UtcNow;
    }
}