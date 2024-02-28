namespace Kero_Auth.Application.UseCases;

using Application.Interfaces;
using Domain.User.Dtos;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Domain.User;

public class UserLoginUseCase : IUserLogInUseCase
{
    private readonly IAuthenticationService _authenticationService;
    public UserLoginUseCase(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<string> ExecuteAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        var user = User.Build(userDto.Email, userDto.Password);
        return await _authenticationService.LogInAsync(user);
    }
}
