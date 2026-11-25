using System.ComponentModel.DataAnnotations;

namespace SoaCa2StardewValley.Dtos;

public class CropDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SeedCost { get; set; }
    public int GrowthDays { get; set; }
    public int? RegrowDays { get; set; }
    public int SellPrice { get; set; }
    public int YieldPerHarvest { get; set; }
    public List<string> Seasons { get; set; } = new();
}

public class CropWriteDto
{
    [Required, StringLength(60, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10000)]
    public int SeedCost { get; set; }

    [Range(1, 28)]
    public int GrowthDays { get; set; }

    // Null for single-harvest crops
    [Range(1, 28)]
    public int? RegrowDays { get; set; }

    [Range(1, 100000)]
    public int SellPrice { get; set; }

    [Range(1, 20)]
    public int YieldPerHarvest { get; set; } = 1;

    // Ids of the seasons this crop grows in
    [Required, MinLength(1)]
    public List<int> SeasonIds { get; set; } = new();
}