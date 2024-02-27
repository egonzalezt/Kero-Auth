namespace Kero_Auth.Application.Services.ServiceCollection;

using Kero_Auth.Application.Interfaces;
using Kero_Auth.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceCollection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>();
    }
}
