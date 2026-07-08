using EcommerceApp.Features.User.DTOs;

namespace EcommerceApp.Features.User.Services;

public interface IUserService
{
    Task<bool> LoginUser(LoginUserRequestDto requestDto);
    Task<bool> RegisterUser(RegisterUserRequestDto requestDto);
    string GenerateTokenString(LoginUserRequestDto requestDto);
}