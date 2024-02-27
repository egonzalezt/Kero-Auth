namespace Kero_Auth.Controllers;

using Responses;
using Microsoft.AspNetCore.Mvc;
using Kero_Auth.Domain.User.Dtos;
using Kero_Auth.Application.Interfaces;

[ApiController]
[Route("/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserRegisterUseCase _userRegisterUseCase;

    public AuthenticationController(ILogger<AuthenticationController> logger, IUserRegisterUseCase userRegisterUseCase)
    {
        _logger = logger;
        _userRegisterUseCase = userRegisterUseCase;
    }

    [HttpPost("createUser")]
    public async Task<ActionResult<UserCreated>> CreateUser(UserDto userDto)
    {
        var user = await _userRegisterUseCase.ExecuteAsync(userDto, CancellationToken.None);
        var response = new UserCreated { Email = user.Email, Id = user.Id };
        return Ok(response);
    }
}
