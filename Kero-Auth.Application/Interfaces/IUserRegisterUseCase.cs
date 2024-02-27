namespace Kero_Auth.Application.Interfaces;

using Domain.User;
using Domain.User.Dtos;

public interface IUserRegisterUseCase
{
    Task<User> ExecuteAsync(UserDto userDto, CancellationToken cancellationToken);
}
