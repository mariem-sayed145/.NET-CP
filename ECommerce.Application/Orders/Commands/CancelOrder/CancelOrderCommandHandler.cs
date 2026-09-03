using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Get order
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(
                nameof(Order),
                request.OrderId);
        }

        // 2. Check if order was paid
        var wasPaid = order.Status == OrderStatus.Paid;

        // 3. Cancel order
        order.Cancel();

        // 4. Restock products if order was paid
        if (wasPaid)
        {
            var productIds = order.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var products = (await _productRepository.GetByIdsAsync(
                productIds,
                cancellationToken))
                .ToDictionary(p => p.Id);

            foreach (var item in order.Items)
            {
                if (products.TryGetValue(
                    item.ProductId,
                    out var product))
                {
                    product.Restock(item.Quantity);
                }
            }
        }

        // 5. Save changes
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
} 