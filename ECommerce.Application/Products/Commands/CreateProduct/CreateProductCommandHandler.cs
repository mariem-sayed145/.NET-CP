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
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        // Check if SKU already exists
        if (await _productRepository.ExistsBySkuAsync(
            request.SKU,
            cancellationToken))
        {
            throw new ValidationException(
                $"Product with SKU '{request.SKU}' already exists.");
        }

        // Create product
        var product = new Product(
            request.Name,
            request.SKU,
            request.Price,
            request.StockQuantity);

        // Save product
        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // Return response
        return product.ToResponse();
    }
}