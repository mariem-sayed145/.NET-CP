using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Customers;
using ECommerce.Application.Customers.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler
    : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (customer is null)
            throw new NotFoundException(nameof(Customer), request.Id);

        return customer.ToResponse();
    }
} 