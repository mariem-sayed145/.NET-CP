using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Customers.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler
    : IRequestHandler<RegisterCustomerCommand, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> Handle(
        RegisterCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (await _customerRepository.ExistsByEmailAsync(
                request.Email,
                cancellationToken))
        {
            throw new ValidationException(
                $"Customer with email '{request.Email}' is already registered.");
        }

        var customer = new Customer(
            request.FullName,
            request.Email,
            request.IsVip);

        await _customerRepository.AddAsync(
            customer,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToResponse();
    }
}