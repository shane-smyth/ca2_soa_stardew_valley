namespace SoaCa2StardewValley.Models;

public class ApplicationUser
{
    public string? DisplayName { get; set; }
    public ICollection<Farm> Farms { get; set; } = new List<Farm>();
}