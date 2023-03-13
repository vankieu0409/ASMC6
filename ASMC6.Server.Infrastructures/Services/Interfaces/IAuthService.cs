using ASMC6.Shared.Dtos;
using ASMC6.Shared.Entities;
using ASMC6.Shared.ViewModels;

namespace ASMC6.Server.Infrastructures.Services.Interfaces;

public interface IAuthService
{
    public Task<UserDto> Login(LoginUserViewModel request);
    public Task<UserDto> RegisterUser(CreateUserViewModel request);
    public Task<AccessTokenDto> RefreshToken();
    string CreateToken(UserDto user);
    RefreshTokenDto CreateRefreshToken();
    public void SetRefreshToken(RefreshTokenDto refreshToken, UserDto user);
    public void CreateRoles(Roles role);
    public void UpdateRoles(Roles role);
    public Task<IList<string>> GetRolesOfUser(Users user);
}