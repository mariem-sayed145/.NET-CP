using MediatR;

namespace ECommerce.Application.Chat.Commands.CreateConversation;

public sealed record CreateConversationCommand(
    int CustomerId) : IRequest<int>;