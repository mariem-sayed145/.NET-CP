using ECommerce.Application.Orders.DTOs;

namespace ECommerce.Application.Orders;

public interface IOrderService
{
    Task<OrderResponse> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderResponse>> GetCustomerOrdersAsync(int customerId, CancellationToken cancellationToken = default);
    Task<CheckoutOrderResponse> CheckoutAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task CancelOrderAsync(int orderId, CancellationToken cancellationToken = default);
}
