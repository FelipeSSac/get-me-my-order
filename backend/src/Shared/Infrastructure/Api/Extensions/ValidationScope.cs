using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Api.Controller.Request.Validator;

namespace Api.Extensions;

public static class ValidationScope
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

        return services;
    }
}