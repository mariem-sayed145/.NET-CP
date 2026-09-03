using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Customers.Commands.UpgradeToVip;

public sealed class UpgradeToVipCommandHandler
    : IRequestHandler<UpgradeToVipCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpgradeToVipCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpgradeToVipCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
            throw new NotFoundException(
                nameof(Customer),
                request.CustomerId);

        customer.UpgradeToVip();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
} 