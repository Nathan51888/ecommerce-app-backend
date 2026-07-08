using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceApp.Features.User.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceApp.Features.User.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<bool> LoginUser(LoginUserRequestDto requestDto)
    {
        var identityUser = await _userManager.FindByEmailAsync(requestDto.Username);
        if (identityUser is null)
            return false;

        return await _userManager.CheckPasswordAsync(identityUser, requestDto.Password);
    }

    public async Task<bool> RegisterUser(RegisterUserRequestDto requestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = requestDto.Username,
            Email = requestDto.Email
        };

        var result = await _userManager.CreateAsync(identityUser, requestDto.Password);
        return result.Succeeded;
    }

    public string GenerateTokenString(LoginUserRequestDto requestDto)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, requestDto.Username),
            new(ClaimTypes.Role, "Admin")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }
}