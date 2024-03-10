namespace Kero_Auth.Infrastructure.NotificationService;

using Domain.SharedKernel;
using Domain.User.Dtos;
using Infrastructure.MessageBroker;
using Infrastructure.MessageBroker.Options;
using MessageBroker.Publisher;
using Microsoft.Extensions.Options;

internal class UserCreatedNotifier(IMessageSender messageSender, IOptions<PublisherConfiguration> options) : INotificationService<UserSignInUriDto>
{
    private readonly PublisherConfiguration _publisherConfiguration = options.Value;
    public void Notify(UserSignInUriDto entity, string messageType)
    {
        var headers = new EventHeaders(messageType, entity.Id);
        messageSender.SendMessage(entity, _publisherConfiguration.UserNotificationsQueue, headers.GetAttributesAsDictionary());
    }
}
