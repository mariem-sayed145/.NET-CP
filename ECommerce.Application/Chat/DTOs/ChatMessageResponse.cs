namespace ECommerce.Application.Chat.DTOs;

public sealed record ChatMessageResponse(
    int Id,
    int ConversationId,
    int SenderCustomerId,
    string Message,
    DateTime SentAt);