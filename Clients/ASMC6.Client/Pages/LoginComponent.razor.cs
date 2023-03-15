using ASMC6.Shared.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using MudBlazor;

using Newtonsoft.Json.Linq;

namespace ASMC6.Client.Pages;

[AllowAnonymous]
public partial class LoginComponent : ComponentBase
{

    public List<string> _messErorrCollection = new();

    private LoginUserViewModel userLogin { get; set; } = new();

    private CreateUserViewModel userRegister { get; set; } = new();

    bool isShow;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    #region userLogin

    private void SetUserLoginName(ChangeEventArgs e)
    {
        userLogin.UserName = e.Value.ToString();
    }

    private void SetUserLoginPassword(ChangeEventArgs e)
    {
        userLogin.Password = e.Value.ToString();
    }

    private async Task ShowAlert(string mess)
    {
        await _jsRuntime.InvokeVoidAsync("alert", mess);
    }
    private bool _processing = false;

    async Task ProcessSomething()
    {
        _processing = true;
        await Task.Delay(2000);
        _processing = false;
    }
    private async void HandleLogin()
    {
        _processing = true;
        // ValidateFormLogin();
        var result = await _authentication.LoginService(userLogin);
        _processing = false;
        if (result.IsSuccessStatusCode)
        {
            await _jsRuntime.InvokeVoidAsync("alert", "Đăng nhập thành công!");

            _navigationManager.NavigateTo("/");
        }
        else
        {
            var obj = JObject.Parse(await result.Content.ReadAsStringAsync());
            var passwordErrors = (JArray)obj["errors"]["Password"];
            var userNameErrors = (JArray)obj["errors"]["UserName"];
            if (userNameErrors != null)
                foreach (string error in userNameErrors)
                    _messErorrCollection.Add(error);

            if (passwordErrors != null)

                foreach (string error in passwordErrors)
                    _messErorrCollection.Add(error);

            await ShowAlert(_messErorrCollection.FirstOrDefault());

            _messErorrCollection.Clear();
        }
    }

    //private void ValidateFormLogin()
    //{
    //    if (userLogin.UserName == "" || string.IsNullOrWhiteSpace(userLogin.UserName))
    //    {
    //        _js.InvokeVoidAsync("alert", "Email không được để trống");
    //    }
    //    if (!_regexEmai.IsMatch(userLogin.UserName))
    //    {
    //        _js.InvokeVoidAsync("alert", "Email không đúng định dạng!");
    //    }


    //    if (string.IsNullOrWhiteSpace(userLogin.Password) || userLogin.Password == "")
    //    {
    //        _js.InvokeVoidAsync("alert", "Password không được để trống");
    //    }
    //    if (!_regexNumber.IsMatch(userLogin.Password))
    //    {
    //        _js.InvokeVoidAsync("alert", "Password phải có ít nhất 1 chữ số");
    //    }
    //    if (userLogin.Password.Length < 8)
    //    {
    //        _js.InvokeVoidAsync("alert", "PPassword phải tù tám ký tự trở lên!");
    //    }
    //}

    #endregion

    #region UserRegister

    private void SetUserRegisterName(ChangeEventArgs e)
    {
        userRegister.FullName = e.Value.ToString();
    }

    private void SetUserRegisterEmail(ChangeEventArgs e)
    {
        userRegister.Email = e.Value.ToString();
    }

    private void SetUserRegisterPassword(ChangeEventArgs e)
    {
        userRegister.Password = e.Value.ToString();
    }
    //private void ValidateFormRegister()
    //{
    //    if (userRegister.FullName == "" || string.IsNullOrWhiteSpace(userRegister.FullName))
    //    {
    //        _js.InvokeVoidAsync("alert", "Tên không được để trống");


    //    }
    //    if (userRegister.Email == "" || string.IsNullOrWhiteSpace(userRegister.Email))
    //    {
    //        _js.InvokeVoidAsync("alert", "Email không được để trống");
    //    }
    //    if (!_regexEmai.IsMatch(userRegister.Email))
    //    {
    //        _js.InvokeVoidAsync("alert", "Email không đúng định dạng!");
    //    }

    //    if (string.IsNullOrWhiteSpace(userRegister.Password) || userRegister.Password == "")
    //    {
    //        _js.InvokeVoidAsync("alert", "Password không được để trống");
    //    }
    //    if (!_regexNumber.IsMatch(userRegister.Password))
    //    {
    //        _js.InvokeVoidAsync("alert", "Password phải có ít nhất 1 chữ số");
    //    }
    //    if (userRegister.Password.Length < 8)
    //    {
    //        _js.InvokeVoidAsync("alert", "Password phải tù tám ký tự trở lên!");
    //    }
    //}

    private async void RegisterHandle()
    {
        var isAccessed = await _authentication.RegiterService(userRegister);
        await _jsRuntime.InvokeVoidAsync("alert", "Đăng ký thành công");
        if (isAccessed) _navigationManager.NavigateTo("/login");
    }
    void ButtonTestclick()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
    #endregion
}