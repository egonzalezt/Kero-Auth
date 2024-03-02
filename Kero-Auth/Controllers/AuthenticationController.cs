namespace Kero_Auth.Controllers;

using Responses;
using Microsoft.AspNetCore.Mvc;
using Kero_Auth.Domain.User.Dtos;
using Kero_Auth.Application.Interfaces;
using Kero_Auth.Infrastructure.Services.Filters;

[ApiController]
[Route("/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserRegisterUseCase _userRegisterUseCase;
    private readonly IUserLogInUseCase _userLogInUseCase;
    private readonly IPasswordChangeUseCase _passwordChangeUseCase;

    public AuthenticationController(
        ILogger<AuthenticationController> logger, 
        IUserRegisterUseCase userRegisterUseCase, 
        IUserLogInUseCase userLogInUseCase,
        IPasswordChangeUseCase passwordChangeUseCase
        )
    {
        _logger = logger;
        _userRegisterUseCase = userRegisterUseCase;
        _userLogInUseCase = userLogInUseCase;
        _passwordChangeUseCase = passwordChangeUseCase;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userDto">The user data.</param>
    /// <returns>Returns the created user data.</returns>
    /// <response code="200">Returns the created user data.</response>
    /// <response code="400">If the password is weak, email already exists, or email is missing.</response>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(UserCreated), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserCreated>> CreateUser(UserDto userDto)
    {
        var user = await _userRegisterUseCase.ExecuteAsync(userDto, CancellationToken.None);
        var response = new UserCreated { Email = user.Email, Id = user.Id };
        return Ok(response);
    }


    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="userDto">The user data.</param>
    /// <returns>Returns a token upon successful login.</returns>
    /// <response code="200">Returns a token upon successful login.</response>
    /// <response code="400">If the email format is invalid or the email is missing.</response>
    /// <response code="401">If the password is incorrect or the user account is disabled.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="429">If there are too many login attempts.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<UserAuthenticated>> LoginUser(UserDto userDto)
    {
        var token = await _userLogInUseCase.ExecuteAsync(userDto, CancellationToken.None);
        var response = new UserAuthenticated
        {
            AccessToken = token
        };
        return Ok(response);
    }

    /// <summary>
    /// Sends email for user password change
    /// </summary>
    /// <param name="EmailDto">User email.</param>
    /// <returns>Returns a message notifying that the email was send</returns>
    /// <response code="200">Email sent</response>
    /// <response code="404">User Not found</response>
    /// <response code="400">If the email format is invalid or the email is missing.</response>
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordChangeResponse>> ChangePassword(EmailDto email)
    {
        var noReplyEmail = await _passwordChangeUseCase.ExecuteAsync(email.Email);
        var response = PasswordChangeResponse.GenerateMessage(noReplyEmail);
        return Ok(response);
    }

    [HttpGet("validate-token")]
    [KeroAuthorize]
    public IActionResult ValidateToken()
    {
        return Ok(new { isValid = true });
    }
}
