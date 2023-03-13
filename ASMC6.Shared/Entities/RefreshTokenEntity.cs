using EF.Support.Entities.Interfaces;

namespace ASMC6.Shared.Entities;

public class RefreshTokenEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public bool IsUsed { get; set; } = false;

}