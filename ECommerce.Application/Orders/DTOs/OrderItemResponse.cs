namespace ECommerce.Application.Orders.DTOs;

public record OrderItemResponse(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);
