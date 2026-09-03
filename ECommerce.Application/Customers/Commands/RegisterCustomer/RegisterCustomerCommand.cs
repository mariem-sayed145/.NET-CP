using ECommerce.Application.Customers.DTOs;
using MediatR;

namespace ECommerce.Application.Customers.Commands.RegisterCustomer;

public record RegisterCustomerCommand(
    string FullName,
    string Email,
    bool IsVip
) : IRequest<CustomerResponse>; 