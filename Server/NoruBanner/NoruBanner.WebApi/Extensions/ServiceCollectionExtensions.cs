using NoruBanner.WebApi.Infrastructure.Persistence.Repositories;
using NoruBanner.WebApi.Infrastructure.Persistence;
using NoruBanner.WebApi.Shared.Interfaces;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NoruBanner.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add logging
        services.AddLogging();

        // Configure DbContext
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<AppDbContext>>();
            options.UseNpgsql(
                configuration.GetConnectionString("Default"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
               
        services.AddScoped<IBannerEventRepository, BannerEventRepository>();
        services.AddScoped<IBannerStatisticsRepository, BannerStatisticsRepository>();

        return services;
    }
}
