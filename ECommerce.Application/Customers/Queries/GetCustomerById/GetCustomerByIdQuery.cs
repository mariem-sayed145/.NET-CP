using ECommerce.Application.Customers.DTOs;
using MediatR;

namespace ECommerce.Application.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(int Id) : IRequest<CustomerResponse>; 