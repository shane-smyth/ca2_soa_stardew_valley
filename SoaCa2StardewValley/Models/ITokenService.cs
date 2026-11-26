namespace SoaCa2StardewValley.Models;

// Creates signed JWTs
public interface ITokenService
{
    // Builds a token for the user containing their id, email and roles
    (string Token, DateTime ExpiresAtUtc) CreateToken(ApplicationUser user, IList<string> roles);
}
