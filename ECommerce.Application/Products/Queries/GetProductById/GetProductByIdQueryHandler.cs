using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cache;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        ICacheService cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<ProductResponse> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{request.Id}";

        // 1. Try Redis
        var cachedProduct =
            await _cache.GetAsync<ProductResponse>(cacheKey);

        // 2. Cache Hit
        if (cachedProduct is not null)
        {
            return cachedProduct;
        }

        // 3. Cache Miss → Database
        var product = await _productRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                nameof(Product),
                request.Id);
        }

        var response = product.ToResponse();

        // 4. Save in Redis
        await _cache.SetAsync(
            cacheKey,
            response,
            TimeSpan.FromMinutes(10));

        // 5. Return
        return response;
    }
}
