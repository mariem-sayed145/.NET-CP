using ECommerce.Application.Orders.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Commands.CheckoutOrder;

public record CheckoutOrderCommand(
    int CustomerId,
    List<OrderItemRequest> Items,
    string? CouponCode
) : IRequest<CheckoutOrderResponse>; 
