using MediatR;

namespace ECommerce.Application.Customers.Commands.UpgradeToVip;

public record UpgradeToVipCommand(int CustomerId) : IRequest; 