namespace SoaCa2StardewValley.Models;

public class PlanEntry
{
    public int Id { get; set; }

    public int Quantity { get; set; }
    public int DayPlanted { get; set; }

    // True when this planting was paid for by earlier harvests, not the starting gold.
    public bool IsReinvestment { get; set; }

    // Total revenue minus seed cost for this planting.
    public int ExpectedProfit { get; set; }

    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public int CropId { get; set; }
    public Crop Crop { get; set; } = null!;
}