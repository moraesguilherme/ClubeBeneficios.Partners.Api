using Microsoft.AspNetCore.Authorization;

namespace ClubeBeneficios.Partners.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
            {
                policy.AddAuthenticationSchemes("PartnersJwt");
                policy.RequireAuthenticatedUser();
                policy.RequireRole("admin");
            });

            options.AddPolicy("PartnerOnly", policy =>
            {
                policy.AddAuthenticationSchemes("PartnersJwt");
                policy.RequireAuthenticatedUser();
                policy.RequireRole("partner");
            });

            options.AddPolicy("PartnerOrAdmin", policy =>
            {
                policy.AddAuthenticationSchemes("PartnersJwt");
                policy.RequireAuthenticatedUser();
                policy.RequireRole("partner", "admin");
            });
        });

        return services;
    }
}