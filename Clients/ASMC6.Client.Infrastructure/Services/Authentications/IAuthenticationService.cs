using ASMC6.Shared.ViewModels;

namespace ASMC6.Client.Infrastructure.Services.Authentications;

public interface IAuthenticationService
{
    public Task<HttpResponseMessage> LoginService(LoginUserViewModel viewModel);
    public Task<bool> RegiterService(CreateUserViewModel viewModel);
    public Task<bool> LogoutService();
}