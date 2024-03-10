namespace Kero_Auth.Domain.SharedKernel;

public interface INotificationService<in T> where T : class
{
    void Notify(T entity, string messageType);
}