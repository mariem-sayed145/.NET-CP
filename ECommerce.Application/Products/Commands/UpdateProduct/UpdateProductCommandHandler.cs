using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateProductCommand request,
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

        product.UpdateDetails(
            request.Name,
            request.SKU,
            request.Price);

        _productRepository.Update(product);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}