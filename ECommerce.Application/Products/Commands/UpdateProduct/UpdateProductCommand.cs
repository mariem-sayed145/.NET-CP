using ECommerce.Application.Products.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string Name,
    string SKU,
    decimal Price,
    int StockQuantity
) : IRequest; 