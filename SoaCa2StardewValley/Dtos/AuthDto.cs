using System.ComponentModel.DataAnnotations;

namespace SoaCa2StardewValley.Dtos;

// POST /api/auth/register
public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? DisplayName { get; set; }
}

// POST /api/auth/login
public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

// Returned after a successful register or login. The client sends Token as "Authorization: Bearer ...
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}