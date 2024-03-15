namespace Kero_Auth.Application.UseCases;

using Application.Interfaces;
using Domain.Authentication;
using Domain.SharedKernel;
using Domain.User;
using Domain.User.Dtos;
using Microsoft.Extensions.Logging;

public class UserRegisterUseCase(IAuthenticationService authenticationService, ILogger<UserRegisterUseCase> logger, INotificationService<UserSignInUriDto> notificationService) : IUserRegisterUseCase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<User> CreateWithPasswordExecuteAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("The registration for the user begins");
        var user = User.Build(userDto.Email, userDto.Password);
        var id = await _authenticationService.SignUpAsync(user, cancellationToken);
        user.SetId(Guid.Parse(id));
        logger.LogInformation("Registration complete, new user created with Id {id}", id);
        await GenerateUserNotificationAsync(user.Email, user.Id);
        return user;
    }

    public async Task<User> CreatePasswordLessAsync(UserOwnedDto userDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("The registration for the user begins");
        var user = User.Build(userDto.Email);
        user.SetName(userDto.Name);
        user.SetId(userDto.Id);
        var id = await _authenticationService.SignUpWithoutPasswordAsync(user, cancellationToken);
        var userExists = await _authenticationService.EmailExistsAsync(user.Email);
        if (!userExists)
        {
            await _authenticationService.SignUpWithoutPasswordAsync(user, cancellationToken);
        }
        logger.LogInformation("Registration complete, new user created with Id {id}", id);
        await GenerateUserNotificationAsync(user.Email, user.Id);
        return user;
    }

    private async Task GenerateUserNotificationAsync(string email, Guid id)
    {
        logger.LogInformation("Sending notification message");
        var passwordSetUri = await _authenticationService.GeneratePasswordResetAsync(email);
        var verificationEmailUri = await _authenticationService.GenerateVerificationUrlAsync(email);
        var userSignInDto = new UserSignInUriDto
        {
            Id = id,
            PasswordSetUri = passwordSetUri,
            VerificationEmailUri = verificationEmailUri,
            Email = email,
        };
        notificationService.Notify(userSignInDto, UserOperations.FirstSignIn.ToString());
    }
}
