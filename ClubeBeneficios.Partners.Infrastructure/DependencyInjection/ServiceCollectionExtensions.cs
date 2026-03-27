using Microsoft.Extensions.DependencyInjection;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;
using ClubeBeneficios.Partners.Infrastructure.Context;
using ClubeBeneficios.Partners.Infrastructure.Repositories;
using ClubeBeneficios.Partners.Infrastructure.Services;

namespace ClubeBeneficios.Partners.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<DbConnectionFactory>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPartnerService, PartnerService>();
        return services;
    }
}
