namespace EcommerceApp.Features.User.DTOs;

public class RegisterUserResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}