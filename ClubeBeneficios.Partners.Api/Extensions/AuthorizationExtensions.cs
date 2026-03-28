namespace ClubeBeneficios.Partners.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            options.AddPolicy("PartnerOnly", policy => policy.RequireRole("partner"));
            options.AddPolicy("PartnerOrAdmin", policy => policy.RequireRole("partner", "admin"));
        });

        return services;
    }
}