namespace SoaCa2StardewValley.Models;

public class PlanEntry
{
    public int Id { get; set; }

    public int Quantity { get; set; }
    public int DayPlanted { get; set; }

    /// <summary>Day of the first harvest (DayPlanted + the crop's GrowthDays).</summary>
    public int FirstHarvestDay { get; set; }

    /// <summary>How many harvests this planting yields before the season ends.</summary>
    public int HarvestCount { get; set; }

    public int ExpectedCost { get; set; }
    public int ExpectedRevenue { get; set; }

    public bool IsReinvestment { get; set; }

    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public int CropId { get; set; }
    public Crop Crop { get; set; } = null!;
}