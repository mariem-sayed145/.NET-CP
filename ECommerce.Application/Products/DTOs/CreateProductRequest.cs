namespace ECommerce.Application.Products.DTOs;

public record CreateProductRequest(string Name, string SKU, decimal Price, int StockQuantity);
