using ECommerce.Application.Chat.DTOs;
using MediatR;

namespace ECommerce.Application.Chat.Queries.GetConversation;

public sealed record GetConversationQuery(
    int ConversationId) : IRequest<IReadOnlyList<ChatMessageResponse>>;