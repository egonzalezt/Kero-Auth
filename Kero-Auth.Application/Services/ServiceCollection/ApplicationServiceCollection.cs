namespace Kero_Auth.Application.Services.ServiceCollection;

using Interfaces;
using UseCases;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceCollection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>();
        services.AddScoped<IUserLogInUseCase, UserLoginUseCase>();
        services.AddScoped<IPasswordChangeUseCase, PasswordChangeUseCase>();
        services.AddScoped<IUnregisterUserUserCase, UnregisterUserUserCase>();
    }
}
