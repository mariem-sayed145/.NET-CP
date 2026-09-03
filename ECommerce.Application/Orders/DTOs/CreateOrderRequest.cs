namespace ECommerce.Application.Orders.DTOs;

public record CreateOrderRequest(int CustomerId, List<OrderItemRequest> Items, string? CouponCode);
