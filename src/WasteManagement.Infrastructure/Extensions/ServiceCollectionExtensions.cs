using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WasteManagement.Application.Abstractions;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.Security;
using WasteManagement.Infrastructure.Persistence;
using WasteManagement.Infrastructure.Seed;
using WasteManagement.Infrastructure.Services;

namespace WasteManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

        services.AddDbContext<WasteManagementDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICollectionPointService, CollectionPointService>();
        services.AddScoped<ICollectionRequestService, CollectionRequestService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IImpactReportService, ImpactReportService>();

        return services;
    }

    public static async Task<WebApplication> ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WasteManagementDbContext>();
        var providerName = dbContext.Database.ProviderName ?? string.Empty;
        if (providerName.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        else
        {
            await dbContext.Database.MigrateAsync();
        }
        await DatabaseSeeder.SeedAsync(dbContext);
        return app;
    }
}
