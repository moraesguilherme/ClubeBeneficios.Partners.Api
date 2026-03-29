using System.Text;
using ClubeBeneficios.Partners.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ClubeBeneficios.Partners.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>();

        if (jwtSettings is null || string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
        {
            throw new InvalidOperationException("A configuração JWT não foi encontrada ou está inválida.");
        }

        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "PartnersJwt";
            options.DefaultChallengeScheme = "PartnersJwt";
        })
        .AddJwtBearer("PartnersJwt", options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.MapInboundClaims = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                NameClaimType = "sub",
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();

                    Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | Authorization={authHeader}");

                    if (!string.IsNullOrWhiteSpace(authHeader) &&
                        authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = authHeader["Bearer ".Length..].Trim();
                        Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | ExtractedToken={token}");
                        Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | DotCount={token.Count(c => c == '.')}");

                        context.Token = token;
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var sub = context.Principal?.FindFirst("sub")?.Value;
                    var role = context.Principal?.FindFirst("role")?.Value;

                    Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | Token validated | sub={sub} | role={role}");

                    foreach (var claim in context.Principal?.Claims ?? Enumerable.Empty<System.Security.Claims.Claim>())
                    {
                        Console.WriteLine($"[JWT] CLAIM => {claim.Type} = {claim.Value}");
                    }

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | Authentication failed: {context.Exception}");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine($"[JWT] Scheme=PartnersJwt | Path={context.Request.Path} | Challenge: error={context.Error}, description={context.ErrorDescription}");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}