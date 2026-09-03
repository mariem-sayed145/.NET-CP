using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Products.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductResponse>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(
            cancellationToken);

        return products.ToResponseList();
    }
} 