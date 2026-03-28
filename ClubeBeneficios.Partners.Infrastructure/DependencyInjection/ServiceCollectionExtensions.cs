using ClubeBeneficios.Partners.Domain.Interfaces;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;
using ClubeBeneficios.Partners.Infrastructure.Context;
using ClubeBeneficios.Partners.Infrastructure.Repositories;
using ClubeBeneficios.Partners.Infrastructure.Security;
using ClubeBeneficios.Partners.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClubeBeneficios.Partners.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<DbConnectionFactory>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IUserContext, CurrentUserContext>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPartnerService, PartnerService>();
        return services;
    }
}