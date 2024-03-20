namespace Kero_Auth.Application.UseCases; 

using Application.Interfaces;
using Domain.User.Dtos;
using Domain.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

internal class UnregisterUserUserCase(IAuthenticationService authenticationService, ILogger<UnregisterUserUserCase> logger) : IUnregisterUserUserCase
{
    public async Task<bool> ExecuteAsync(UserTransferRequestDto userTransferRequestDto)
    {
        var user = await authenticationService.GetUserAsync(userTransferRequestDto.UserId);
        if (user == null)
        {
            return false;
        }
        if (user.Disabled)
        {
            logger.LogInformation("User already disabled");
            return true;
        }
        return await authenticationService.DeactivateUserAsync(userTransferRequestDto.UserId);
    }
}
