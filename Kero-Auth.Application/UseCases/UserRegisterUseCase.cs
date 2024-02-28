namespace Kero_Auth.Application.UseCases;

using Kero_Auth.Application.Interfaces;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Domain.User;
using Kero_Auth.Domain.User.Dtos;
using Microsoft.Extensions.Logging;

public class UserRegisterUseCase : IUserRegisterUseCase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<UserRegisterUseCase> _logger;
    public UserRegisterUseCase(IAuthenticationService authenticationService, ILogger<UserRegisterUseCase> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<User> ExecuteAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("The registration for the user begins");
        var user = User.Build(userDto.Email, userDto.Password);
        var id = await _authenticationService.SignUpAsync(user);
        user.SetId(id);
        _logger.LogInformation("Registration complete, new user created with Id {id}", id);
        return user;
    }
}
