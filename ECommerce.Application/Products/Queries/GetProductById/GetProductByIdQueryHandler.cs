using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                nameof(Product),
                request.Id);
        }

        return product.ToResponse();
    }
} 