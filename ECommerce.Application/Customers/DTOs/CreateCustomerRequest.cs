namespace ECommerce.Application.Customers.DTOs;

public record CreateCustomerRequest(string FullName, string Email, bool IsVip);
