using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteProductCommand request,
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

        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
} 