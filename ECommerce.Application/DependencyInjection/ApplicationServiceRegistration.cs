using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        return services;
    }
} 