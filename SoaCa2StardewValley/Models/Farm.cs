namespace SoaCa2StardewValley.Models;

public class Farm
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int TileCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public required string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = null!;

    public ICollection<Plan> Plans { get; set; } = new List<Plan>();
}