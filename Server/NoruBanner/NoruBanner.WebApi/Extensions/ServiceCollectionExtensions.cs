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
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));
               
        services.AddScoped<IBannerEventRepository, BannerEventRepository>();
        services.AddScoped<IBannerStatisticsRepository, BannerStatisticsRepository>();

        return services;
    }
}
