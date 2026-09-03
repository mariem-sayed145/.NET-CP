using MediatR;

namespace ECommerce.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(
    int OrderId
) : IRequest; 
