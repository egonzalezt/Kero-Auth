namespace Kero_Auth.Infrastructure.MessageBroker.Options;

public class PublisherConfiguration
{
    public string UserNotificationsQueue { get; set; }
    public string UserTransferReplyQueue { get; set; }
}
