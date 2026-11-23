namespace SoaCa2StardewValley.Models;

public enum PlanStrategy
{
    Greedy = 0,
    DynamicProgramming = 1
}

/// <summary>
/// A saved planting plan: the inputs given by the user plus the optimiser's projected result.
/// One plan has many entries (one-to-many).
/// </summary>
public class Plan
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // ---- Inputs ----
    public int StartSeasonId { get; set; }
    public Season StartSeason { get; set; } = null!;

    /// <summary>Day of the season the plan starts on (1 to 28).</summary>
    public int StartDay { get; set; }

    public int StartingGold { get; set; }
    public PlanStrategy Strategy { get; set; }

    /// <summary>
    /// When true the simulation sells each harvest and replants with the earnings
    /// (compounding, buying better seeds) until the season ends.
    /// When false it plants once on the start day only.
    /// </summary>
    public bool ReinvestEarnings { get; set; }

    // ---- Projected results ----
    public int ProjectedFinalGold { get; set; }
    public int ProjectedProfit { get; set; }

    public int FarmId { get; set; }
    public Farm Farm { get; set; } = null!;

    public ICollection<PlanEntry> Entries { get; set; } = new List<PlanEntry>();
}
