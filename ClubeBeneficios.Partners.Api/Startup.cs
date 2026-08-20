using ClubeBeneficios.Partners.Api.Extensions;
using ClubeBeneficios.Partners.Infrastructure.Clients.Identity;
using ClubeBeneficios.Partners.Infrastructure.DependencyInjection;
using Dapper;

namespace ClubeBeneficios.Partners.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddApiControllers();
        services.AddApiSwagger();
        services.AddApiCors();
        services.AddApiAuthentication(Configuration);
        services.AddApiAuthorization();
        services.AddInfrastructure();
        services.AddApplicationServices();
        services.AddHttpClient<IIdentityPartnerInvitationClient, IdentityPartnerInvitationClient>((serviceProvider, client) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var baseUrl = configuration["IdentityApi:BaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("IdentityApi:BaseUrl não configurada.");
            }

            client.BaseAddress = new Uri(baseUrl);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseApiExceptionHandling();
        app.UseApiSwagger();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("DefaultPolicy");
        app.UseAuthentication();
        app.UseUserContext();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}