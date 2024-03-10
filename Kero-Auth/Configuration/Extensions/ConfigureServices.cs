namespace Kero_Auth.Configuration.Extensions;

using Infrastructure.Services.ServiceCollection;
using Application.Services.ServiceCollection;
using Workers.Extensions;
using Frieren_Guard.Extensions;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
        services.AddFrierenGuardServices(configuration);
        services.AddInfrastructure(configuration);
        services.AddWorkers(configuration);
        services.AddApplication();
    }
}
