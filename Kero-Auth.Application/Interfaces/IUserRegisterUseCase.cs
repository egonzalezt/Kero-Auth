namespace Kero_Auth.Application.Interfaces;

using Domain.User;
using Domain.User.Dtos;

public interface IUserRegisterUseCase
{
    Task<User> CreateWithPasswordExecuteAsync(UserDto userDto, CancellationToken cancellationToken);
    Task<User> CreatePasswordLessAsync(UserOwnedDto userDto, CancellationToken cancellationToken);
}
