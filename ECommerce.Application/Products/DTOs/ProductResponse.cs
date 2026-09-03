namespace ECommerce.Application.Products.DTOs;

public record ProductResponse(int Id, string Name, string SKU, decimal Price, int StockQuantity);
