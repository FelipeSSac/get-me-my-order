namespace Worker.Extensions;

public static class CorsScope
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(
                    configuration["CORS_ORIGINS"]?.Split(",") ?? new[] { "http://localhost:3000" }
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        return services;
    }
}
