namespace Kero_Auth.Workers.Extensions;

using RabbitMQ.Client;
using MessageBroker.Options;
using Kero_Auth.Workers.MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConnectionFactory>(sp =>
        {
            var factory = new ConnectionFactory();
            configuration.GetSection("RabbitMQ:Connection").Bind(factory);
            return factory;
        });
        services.Configure<ConsumerConfiguration>(configuration.GetSection("RabbitMQ:Queues:Consumer"));
        services.AddHostedService<UserConsumerWorker>();
    }
}