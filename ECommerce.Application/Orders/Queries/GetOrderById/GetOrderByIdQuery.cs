using ECommerce.Application.Orders.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(
    int Id
) : IRequest<OrderResponse>; 
