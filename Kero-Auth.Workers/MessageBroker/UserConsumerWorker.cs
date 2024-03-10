namespace Kero_Auth.Workers.MessageBroker;

using Frieren_Guard;
using Domain.User.Dtos;
using Domain.User;
using Workers.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Workers.Extensions;
using Workers.MessageBroker.Options;
using Application.Interfaces;

internal class UserConsumerWorker(
    ILogger<UserConsumerWorker> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.UserOwnedQueue)
{
    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel, CancellationToken stoppingToken)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if (operation is UserOperations.CreateUser)
        {
            var userDto = JsonSerializer.Deserialize<UserOwnedDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing request for user {userId}", userDto.IdentificationNumber);
            var registerUserUseCase = scope.ServiceProvider.GetRequiredService<IUserRegisterUseCase>();
            await registerUserUseCase.CreatePasswordLessAsync(userDto, stoppingToken);
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }
}
