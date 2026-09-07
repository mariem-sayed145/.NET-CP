using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Products.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductResponse>>
{
    private const string CacheKey = "products:all";

    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cache;

    public GetAllProductsQueryHandler(
        IProductRepository productRepository,
        ICacheService cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ProductResponse>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Try to get products from Redis
        var cachedProducts =
            await _cache.GetAsync<List<ProductResponse>>(CacheKey);

        // 2. Cache Hit
        if (cachedProducts is not null)
        {
            return cachedProducts;
        }

        // 3. Cache Miss → Get data from database
        var products = await _productRepository.GetAllAsync(
            cancellationToken);

        var response = products.ToResponseList();

        // 4. Store the result in Redis
        await _cache.SetAsync(
            CacheKey,
            response,
            TimeSpan.FromMinutes(10));

        // 5. Return data
        return response;
    }
}
