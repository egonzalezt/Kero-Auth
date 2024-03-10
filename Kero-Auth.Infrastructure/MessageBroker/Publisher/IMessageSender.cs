namespace Kero_Auth.Infrastructure.MessageBroker.Publisher;

using System.Collections.Generic;

public interface IMessageSender
{
    void SendMessage<T>(T message, string queueName, IDictionary<string, object>? headers = null);
    void SendBroadcast<T>(T message, string exchangeName, IDictionary<string, object>? headers = null);
}
