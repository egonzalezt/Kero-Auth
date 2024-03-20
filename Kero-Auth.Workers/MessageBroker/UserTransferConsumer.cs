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
using Infrastructure.MessageBroker.Options;
using Infrastructure.MessageBroker;

internal class UserTransferConsumer(
    ILogger<UserTransferConsumer> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues,
    IOptions<PublisherConfiguration> publisherQueue
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.UserTransferQueue)
{

    public readonly PublisherConfiguration PublisherConfiguration = publisherQueue.Value;

    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel, CancellationToken stoppingToken)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if (operation is UserOperations.TransferUser)
        {
            var userDto = JsonSerializer.Deserialize<UserTransferRequestDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing request for user {userId}", userDto.UserId);
            var registerUserUseCase = scope.ServiceProvider.GetRequiredService<IUnregisterUserUserCase>();
            await registerUserUseCase.ExecuteAsync(userDto);
            PublishUnregisterUserRequestToProxy(userDto.UserId, userDto, channel);
            logger.LogInformation("user {userId} is not part of Kero now", userDto.UserId);
        }
        channel.BasicAck(eventArgs.DeliveryTag, false);
    }

    private void PublishUnregisterUserRequestToProxy(Guid userId, UserTransferRequestDto unRegisterUserDto, IModel channel)
    {
        if (unRegisterUserDto is null)
        {
            logger.LogWarning("User with Id {id} not found on the system", userId);
            return;
        }
        var requestHeaders = new EventHeaders(UserOperations.KeroResponse.ToString(), userId);
        var properties = channel.CreateBasicProperties();
        properties.Headers = requestHeaders.GetAttributesAsDictionary();
        string jsonResult = JsonSerializer.Serialize(unRegisterUserDto);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonResult);
        channel.BasicPublish("", PublisherConfiguration.UserTransferReplyQueue, properties, jsonBytes);

    }
}
