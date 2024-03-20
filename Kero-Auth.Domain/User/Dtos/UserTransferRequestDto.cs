namespace Kero_Auth.Domain.User.Dtos;

public class UserTransferRequestDto
{
    public Guid UserId { get; set; }
    public string OperatorId { get; set; }
    public string UserEmail { get; set; }
}
