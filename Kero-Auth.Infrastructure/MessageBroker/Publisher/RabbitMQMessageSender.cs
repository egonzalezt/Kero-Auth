namespace Kero_Auth.Infrastructure.MessageBroker.Publisher;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public class RabbitMQMessageSender : IMessageSender
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQMessageSender> _logger;
    public RabbitMQMessageSender(ConnectionFactory connectionFactory, ILogger<RabbitMQMessageSender> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public void SendMessage<T>(T message, string queueName, IDictionary<string, object>? headers = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        string serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        var properties = channel.CreateBasicProperties();

        if (headers is not null)
        {
            properties.Headers = headers;
        }

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: properties,
                             body: body);

        _logger.LogInformation("Message sent to queue: {queueName}", queueName);
    }

    public void SendBroadcast<T>(T message, string exchangeName, IDictionary<string, object>? headers = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        string serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        var properties = channel.CreateBasicProperties();

        if (headers is not null)
        {
            properties.Headers = headers;
        }

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: "",
                             basicProperties: properties,
                             body: body);

        _logger.LogInformation("Broadcast message sent on the exchange: {exchangeName}", exchangeName);
    }
}

