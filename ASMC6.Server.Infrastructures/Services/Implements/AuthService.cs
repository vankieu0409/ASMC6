using ASMC6.Server.Infrastructures.Data;
using ASMC6.Server.Infrastructures.Repositories.Interfaces;
using ASMC6.Server.Infrastructures.Services.Interfaces;
using ASMC6.Shared.Dtos;
using ASMC6.Shared.Entities;
using ASMC6.Shared.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ASMC6.Server.Infrastructures.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<Users> _userManager;
    private readonly RoleManager<Roles> _roleManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly IRoleRepository _roleRepository;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(SignInManager<Users> signInManager, UserManager<Users> userManager,
        ApplicationDbContext context, IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration,
         IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<UserDto> Login(LoginUserViewModel viewModel)
    {
        var user = await _userManager.FindByNameAsync(viewModel.UserName);

        if (user == null) return new UserDto { Message = "User not found." };
        if (!await _userManager.CheckPasswordAsync(user, viewModel.Password))
            return new UserDto { Message = "Wrong Password." };
        var signManager = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, lockoutOnFailure: false);
        if (signManager.Succeeded)
        {
            var userDto = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            var token = CreateToken(userDto);
            var refreshToken = CreateRefreshToken();
            SetRefreshToken(refreshToken, userDto);

            return new UserDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires
            };
        }
        else if (signManager.IsLockedOut)
        {
            return new UserDto { Message = "LockOut" };
        }
        else
        {
            return new UserDto { Message = " Invalid login attempt" };
        }

    }

    public async Task<UserDto> RegisterUser(CreateUserViewModel request)
    {
        var user = new Users
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            NormalizedUserName = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = request.Email,
            EmailConfirmed = true,
            NormalizedEmail = request.Email,
            PhoneNumber = "",
            PhoneNumberConfirmed = false,
            LockoutEnabled = false,
            LockoutEnd = DateTimeOffset.MinValue,
            AccessFailedCount = 0,
            IsDeleted = false,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = false
        };
        await _userManager.CreateAsync(user, request.Password);
        await _userManager.AddToRoleAsync(user, "Customer");


        await _context.SaveChangesAsync();
        var userDto = new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,

        };

        return userDto;
    }

    public Task<AccessTokenDto> RefreshToken()
    {
        var userDtoCollection = new List<UserDto>();
        foreach (var Users in _userRepository.AsQueryable())
        {
            var userDto = new UserDto()
            {
                Id = Users.Id,
                UserName = Users.UserName
            };
            userDtoCollection.Add(userDto);
        }

        var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
        var user = userDtoCollection.FirstOrDefault(u => u.RefreshToken == refreshToken);
        if (user == null)
            return Task.FromResult(new AccessTokenDto { Message = "Invalid Refresh Token" });
        else if (user.TokenExpires < DateTime.Now)
            return Task.FromResult(new AccessTokenDto { Message = "Token expired." });

        var token = CreateToken(user);
        var newRefreshToken = CreateRefreshToken();
        SetRefreshToken(newRefreshToken, user);

        return Task.FromResult(new AccessTokenDto
        {
            Success = true,
            Token = token,
            RefreshToken = newRefreshToken.Token,
            TokenExpires = newRefreshToken.Expires
        });
    }

    public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        byte[] test;
        test = Convert.FromBase64String(passwordSalt);
        var hmac = new HMACSHA512(test);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(Encoding.UTF8.GetBytes(passwordHash));
    }

    public string CreatePasswordSalt(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public string CreateToken(UserDto user)
    {
        //List<Claim> claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim(ClaimTypes.Role, "Admin")
        //};

        //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        //    _configuration.GetSection("Jwt:Secret").Value));

        //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //var token = new JwtSecurityToken(
        //    claims: claims,
        //    expires: DateTime.Now.AddDays(1),
        //    signingCredentials: creds);
        var userRoles = _roleRepository.AsQueryable().FirstOrDefault(p =>
            p.Id == _context.UserRoles.Where(c => c.UserId == user.Id).Select(c => c.RoleId).FirstOrDefault());
        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, userRoles.NormalizedName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Jwt:Secret").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(_configuration["Authentication:Jwt:ValidIssuer"], _configuration["Authentication:Jwt:ValidAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt16(_configuration["Authentication:Jwt:ExpiresTime"])),
            signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public RefreshTokenDto CreateRefreshToken()
    {
        var refreshToken = new RefreshTokenDto
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddMinutes(5),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    public async void SetRefreshToken(RefreshTokenDto refreshToken, UserDto user)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expires
        };
        _httpContextAccessor?.HttpContext?.Response
            .Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        user.RefreshToken = refreshToken.Token;
        user.TokenCreated = refreshToken.Created;
        user.TokenExpires = refreshToken.Expires;

        await _userRepository.SaveChangesAsync();
    }

    public void CreateRoles(Roles role)
    {
        _roleManager.CreateAsync(role);
    }

    public void UpdateRoles(Roles role)
    {
        _roleManager.UpdateAsync(role);
    }

    public async Task<IList<string>> GetRolesOfUser(Users user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}