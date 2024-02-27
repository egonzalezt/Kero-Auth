namespace Kero_Auth.Infrastructure.Services.ServiceCollection;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        var appOptions = new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        };
        FirebaseApp.Create(appOptions);

        services.AddSingleton<IAuthenticationService, AuthenticationService>();
    }
}
