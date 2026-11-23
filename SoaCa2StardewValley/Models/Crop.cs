namespace SoaCa2StardewValley.Models;

public class Crop
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int SeedCost { get; set; }
    public int GrowthDays { get; set; }
    public int? RegrowDays { get; set; }
    public int SellPrice { get; set; }
    public int YieldPerHarvest { get; set; } = 1;
    public ICollection<CropSeason> CropSeasons { get; set; } = new List<CropSeason>();
}