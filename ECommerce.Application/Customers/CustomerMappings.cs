using ECommerce.Application.Customers.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Customers;

public static class CustomerMappings
{
    public static CustomerResponse ToResponse(this Customer customer) =>
        new(customer.Id, customer.FullName, customer.Email, customer.IsVip);
}
