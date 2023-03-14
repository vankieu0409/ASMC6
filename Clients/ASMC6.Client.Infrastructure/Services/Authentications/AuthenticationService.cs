using ASMC6.Shared.Dtos;
using ASMC6.Shared.ViewModels;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace ASMC6.Client.Infrastructure.Services.Authentications;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authStateProvider = authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task<HttpResponseMessage> LoginService(LoginUserViewModel viewModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/Authentication/login", viewModel);
        var a = result.Content.Headers;
        var user = await result.Content.ReadFromJsonAsync<UserDto>();


        if (result.IsSuccessStatusCode)
        {
            await _localStorage.SetItemAsync("bearer", user.Token);
            await _localStorage.SetItemAsync("user", user);
            await _authStateProvider.GetAuthenticationStateAsync();
        }
        //else
        //{
        //    JObject obj = JObject.Parse(token);
        //    JArray passwordErrors = (JArray)obj["errors"]["Password"];
        //    JArray userNameErrors = (JArray)obj["errors"]["UserName"];

        //    foreach (string error in userNameErrors)
        //    {
        //        messErorrCollection.Add(error);
        //    }
        //    foreach (string error in passwordErrors)
        //    {
        //        messErorrCollection.Add(error);
        //    }
        //}

        return result;
    }

    public async Task<bool> RegiterService(CreateUserViewModel viewModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/Authentication/register", viewModel);
        return await Task.FromResult(result.IsSuccessStatusCode);
    }

    public async Task<bool> LogoutService()
    {
        await _localStorage.ClearAsync();
        var checkTokenLogout = string.IsNullOrEmpty(await _localStorage.GetItemAsStringAsync("bearer"));
        //_authStateProvider.NotifyAuthenticationStateChangedForLogout();
        return await Task.FromResult(checkTokenLogout);
    }
}