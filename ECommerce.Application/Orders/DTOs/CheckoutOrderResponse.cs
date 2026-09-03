namespace ECommerce.Application.Orders.DTOs;

public record CheckoutOrderResponse(
    int OrderId,
    string Status,
    decimal Subtotal,
    decimal Discount,
    decimal Tax,
    decimal Shipping,
    decimal Total,
    string TransactionReference);
