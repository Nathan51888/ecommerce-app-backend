using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Customer.Data;

public sealed class ApplicationUser : IdentityUser
{
    public bool EnableNotifications { get; set; }
    public string Initials { get; set; } = string.Empty;
}