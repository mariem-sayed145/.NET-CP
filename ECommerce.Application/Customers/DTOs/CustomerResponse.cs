namespace ECommerce.Application.Customers.DTOs;

public record CustomerResponse(int Id, string FullName, string Email, bool IsVip);
