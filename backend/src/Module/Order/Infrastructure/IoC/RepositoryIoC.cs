using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Repository;
using Order.Infrastructure.Persistence.EntityFramework.Repository;

namespace Order.Infrastructure.IOC;

public static class RepositoryIoC
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepositoryEntityFramework>();
        services.AddScoped<IClientRepository, ClientRepositoryEntityFramework>();
        services.AddScoped<IProductRepository, ProductRepositoryEntityFramework>();
        
        return services;
    }
}