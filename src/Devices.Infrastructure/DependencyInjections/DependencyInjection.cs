using Devices.Application.Data;
using Devices.Domain.Devices.Repositories;
using Devices.Infrastructure.Data;
using Devices.Infrastructure.Devices.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Devices.Infrastructure.DependencyInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseServices();
        services.AddServices();

        return services;
    }

    private static void AddDatabaseServices(this IServiceCollection services)
    {
        var connectionStringSettings = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionStringSettings)
                   .EnableDetailedErrors()
                   .LogTo(Console.WriteLine, [RelationalEventId.CommandExecuting, RelationalEventId.CommandError], LogLevel.Information);
        });

        services.MigrateDatabase();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();
        services.AddScoped<IDataContext, DataContext>();

        services.AddScoped<IDeviceRepository, DeviceRepository>();
    }

    private static void MigrateDatabase(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        var context = sp.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
