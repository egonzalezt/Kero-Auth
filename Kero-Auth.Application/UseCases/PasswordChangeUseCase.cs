namespace Kero_Auth.Application.UseCases;
using Interfaces;
using Kero_Auth.Domain.Authentication;
using System.Threading.Tasks;

public class PasswordChangeUseCase : IPasswordChangeUseCase
{
    private readonly IAuthenticationService _authenticationService;

    public PasswordChangeUseCase(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<string> ExecuteAsync(string email)
    {
        return await _authenticationService.ResetPasswordAsync(email);
    }
}
