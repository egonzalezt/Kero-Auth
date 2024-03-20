namespace Kero_Auth.Application.Interfaces;

using Domain.User.Dtos;

public interface IUnregisterUserUserCase
{
    Task<bool> ExecuteAsync(UserTransferRequestDto userTransferRequestDto);
}
