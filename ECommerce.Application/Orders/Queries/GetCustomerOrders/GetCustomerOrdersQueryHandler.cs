using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Orders.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetCustomerOrders;

public sealed class GetCustomerOrdersQueryHandler
    : IRequestHandler<
        GetCustomerOrdersQuery,
        IReadOnlyList<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetCustomerOrdersQueryHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(
            request.CustomerId,
            cancellationToken);

        return orders
            .Select(o => o.ToResponse())
            .ToList()
            .AsReadOnly();
    }
}