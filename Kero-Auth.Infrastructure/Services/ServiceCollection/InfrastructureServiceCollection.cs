namespace Kero_Auth.Infrastructure.Services.ServiceCollection;

using Firebase.Auth.Providers;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Services.Options;
using Newtonsoft.Json;
using Domain.Authentication;
using Infrastructure.Authentication;
using FirebaseAdmin.Auth;
using Infrastructure.MessageBroker.Publisher;
using Domain.SharedKernel;
using Domain.User.Dtos;
using Infrastructure.NotificationService;
using Kero_Auth.Infrastructure.MessageBroker.Options;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.Configure<FirebaseOptions>(configuration.GetSection("Firebase"));
        services.Configure<AuthOptions>(configuration.GetSection("Authentication"));
        services.Configure<NoReplyOptions>(configuration.GetSection("NoReplyOptions"));

        var firebaseOptions = configuration.GetSection("Firebase").Get<FirebaseOptions>();
        var authOptions = configuration.GetSection("Authentication").Get<AuthOptions>();

        var appOptions = new AppOptions()
        {
            Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseOptions))
        };
        FirebaseApp.Create(appOptions);
        services.AddSingleton(FirebaseAuth.DefaultInstance);

        services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = authOptions.ApiKey,
            AuthDomain = $"{firebaseOptions.project_id}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider(),
                new GoogleProvider()
            }
        }));

        services.AddSingleton<IMessageSender, RabbitMQMessageSender>();
        services.AddSingleton<INotificationService<UserSignInUriDto>, UserCreatedNotifier>();
        services.Configure<PublisherConfiguration>(configuration.GetSection("RabbitMQ:Queues:Publisher"));
    }
}
