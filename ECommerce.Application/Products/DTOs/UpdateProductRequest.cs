namespace ECommerce.Application.Products.DTOs;

public record UpdateProductRequest(string Name, string SKU, decimal Price, int StockQuantity);
