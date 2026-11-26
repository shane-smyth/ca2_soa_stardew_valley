using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoaCa2StardewValley.Data;
using SoaCa2StardewValley.Dtos;
using SoaCa2StardewValley.Models;
using SoaCa2StardewValley.Services;

namespace SoaCa2StardewValley.Controllers;

// Registration and login. Both return a JWT the client uses on protected endpoints.
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly ITokenService _tokens;

    public AuthController(UserManager<ApplicationUser> users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    // POST /api/auth/register : create an account with the "User" role. Returns 201 and a token.
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            DisplayName = dto.DisplayName
        };

        // UserManager hashes the password; the plain text is never stored.
        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
                return Conflict(new { message = "An account with this email already exists." });

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        await _users.AddToRoleAsync(user, IdentitySeeder.UserRole);

        var roles = await _users.GetRolesAsync(user);
        return Created(string.Empty, BuildResponse(user, roles));
    }

    // POST /api/auth/login : returns a token, or 401 if the credentials are wrong
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email);

        // Same response for "no such user" and "wrong password" so attackers can't tell which emails exist.
        if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
            return Unauthorized(new { message = "Invalid email or password." });

        var roles = await _users.GetRolesAsync(user);
        return Ok(BuildResponse(user, roles));
    }

    private AuthResponseDto BuildResponse(ApplicationUser user, IList<string> roles)
    {
        var (token, expires) = _tokens.CreateToken(user, roles);
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAtUtc = expires,
            Email = user.Email ?? string.Empty,
            DisplayName = user.DisplayName,
            Roles = roles
        };
    }
}