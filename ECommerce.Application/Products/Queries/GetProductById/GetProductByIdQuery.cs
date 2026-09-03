using ECommerce.Application.Products.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(
    int Id
) : IRequest<ProductResponse>; 