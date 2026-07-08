using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Features.User.DTOs;

public class RegisterUserRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}