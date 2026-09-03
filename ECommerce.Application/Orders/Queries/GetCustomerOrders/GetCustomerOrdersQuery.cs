using ECommerce.Application.Orders.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetCustomerOrders;

public record GetCustomerOrdersQuery(
    int CustomerId
) : IRequest<IReadOnlyList<OrderResponse>>;
