using ECommerce.Application.Customers.DTOs;

namespace ECommerce.Application.Customers;

public interface ICustomerService
{
    Task<CustomerResponse> GetCustomerByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomerResponse> RegisterCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task UpgradeToVipAsync(int customerId, CancellationToken cancellationToken = default);
}
