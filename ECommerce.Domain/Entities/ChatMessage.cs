using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public sealed class ChatMessage : Entity
{
    public int ConversationId { get; private set; }

    public ChatConversation? Conversation { get; private set; }

    public int SenderCustomerId { get; private set; }

    public Customer? SenderCustomer { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public DateTime SentAt { get; private set; }

    private ChatMessage()
    {
    }

    public ChatMessage(
        int conversationId,
        int senderCustomerId,
        string message)
    {
        if (conversationId <= 0)
            throw new ArgumentException("Invalid Conversation ID.");

        if (senderCustomerId <= 0)
            throw new ArgumentException("Invalid Sender Customer ID.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        ConversationId = conversationId;
        SenderCustomerId = senderCustomerId;
        Message = message.Trim();
        SentAt = DateTime.UtcNow;
    }
}