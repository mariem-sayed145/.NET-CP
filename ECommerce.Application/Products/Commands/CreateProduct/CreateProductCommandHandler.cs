using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private const string AllProductsCacheKey = "products:all";

    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<ProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        if (await _productRepository.ExistsBySkuAsync(
            request.SKU,
            cancellationToken))
        {
            throw new ValidationException(
                $"Product with SKU '{request.SKU}' already exists.");
        }

        var product = new Product(
            request.Name,
            request.SKU,
            request.Price,
            request.StockQuantity);

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // The products list is now outdated.
        await _cache.RemoveAsync(AllProductsCacheKey);

        return product.ToResponse();
    }
}
