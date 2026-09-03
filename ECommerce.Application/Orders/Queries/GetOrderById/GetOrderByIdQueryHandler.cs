using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Orders.DTOs;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(
                nameof(Order),
                request.Id);
        }

        return order.ToResponse();
    }
}
