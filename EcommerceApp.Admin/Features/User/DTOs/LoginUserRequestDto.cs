using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Features.User.DTOs;

public class LoginUserRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}