namespace Kero_Auth.Application.Interfaces;

using Domain.User.Dtos;

public interface IUserLogInUseCase
{
    Task<string> ExecuteAsync(UserDto userDto, CancellationToken cancellationToken);
}
