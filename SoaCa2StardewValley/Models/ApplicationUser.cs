using Microsoft.AspNetCore.Identity;

namespace SoaCa2StardewValley.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public ICollection<Plan> Plans { get; set; } = new List<Plan>();
}