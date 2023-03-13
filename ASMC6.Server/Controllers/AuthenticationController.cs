using ASMC6.Shared.Dtos;
using ASMC6.Shared.ViewModels;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using IAuthenticationService = ASMC6.Server.Infrastructures.Services.Authentications.IAuthenticationService;

namespace ASMC6.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> RegisterAsync(CreateUserViewModel request)
        {
            var response = await _authenticationService.RegisterUser(request);
            return Ok(response);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginUserViewModel request)
        {
            var response = await _authenticationService.Login(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var response = await _authenticationService.RefreshToken();
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }


        [HttpGet("/signin-google")]
        public async Task<IActionResult> SignInGoogle()
        {
            var result = await HttpContext.AuthenticateAsync("External");
            var claims = result.Principal.Claims.ToList();
            // Access the user's information from the claims
            // ...
            return Ok();
        }
    }
}
