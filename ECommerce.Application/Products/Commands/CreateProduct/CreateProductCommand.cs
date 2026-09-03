using ECommerce.Application.Products.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string SKU,
    decimal Price,
    int StockQuantity
) : IRequest<ProductResponse>; 