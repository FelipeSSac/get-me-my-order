using Microsoft.Extensions.DependencyInjection;
using Order.Application.UseCase;
using Order.Application.UseCase.Interface;

namespace Order.Infrastructure.IOC;

public static class UseCaseIoC
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
        services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
        services.AddScoped<IGetOrdersUseCase, GetOrdersUseCase>();

        services.AddScoped<ICreateClientUseCase, CreateClientUseCase>();

        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();

        return services;
    }
}