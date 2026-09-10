using ECommerce.Application.Chat.DTOs;
using MediatR;

namespace ECommerce.Application.Chat.Commands.SendMessage;

public sealed record SendMessageCommand(
    int ConversationId,
    int SenderCustomerId,
    string Message) : IRequest<ChatMessageResponse>;