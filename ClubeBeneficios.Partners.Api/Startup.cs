using ClubeBeneficios.Partners.Api.Extensions;
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