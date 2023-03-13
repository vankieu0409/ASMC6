using ASMC6.Shared.Dtos;
using ASMC6.Shared.Entities;
using ASMC6.Shared.ViewModels;

namespace ASMC6.Server.Infrastructures.Services.Authentications;

public interface IAuthenticationService
{
    public Task<UserDto> Login(LoginUserViewModel request);
    public Task<UserDto> RegisterUser(CreateUserViewModel request);
    public Task<AccessTokenDto> RefreshToken();
    string CreateToken(UserEntity user);
    RefreshTokenDto CreateRefreshToken();
    public void SetRefreshToken(RefreshTokenDto refreshToken, UserEntity user);
    public void CreateRoles(RoleEntity role);
    public void UpdateRoles(RoleEntity role);
    public Task<IList<string>> GetRolesOfUser(UserEntity user);
}