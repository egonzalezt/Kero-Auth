namespace Kero_Auth.Application.Interfaces;

public interface IPasswordChangeUseCase
{
    public Task<string> ExecuteAsync(string email);
}
