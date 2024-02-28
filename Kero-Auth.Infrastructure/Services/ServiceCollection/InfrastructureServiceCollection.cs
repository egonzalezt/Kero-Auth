namespace Kero_Auth.Infrastructure.Services.ServiceCollection;

using Firebase.Auth.Providers;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        };
        FirebaseApp.Create(appOptions);

        var apiKey = configuration.GetValue<string>("Authentication:ApiKey");
        var projectId = configuration.GetValue<string>("Authentication:ProjectId");

        services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = apiKey,
            AuthDomain = $"{projectId}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider(),
                new GoogleProvider()
            }
        }));

        services.AddSingleton<Domain.Authentication.IAuthenticationService, Authentication.AuthenticationService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"https://securetoken.google.com/{projectId}";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = $"https://securetoken.google.com/{projectId}",
                ValidateAudience = true,
                ValidAudience = projectId,
                ValidateLifetime = true
            };
        });
    }
}
