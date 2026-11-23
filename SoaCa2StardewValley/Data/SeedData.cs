using Microsoft.EntityFrameworkCore;
using SoaCa2StardewValley.Models; // change if your models use a different namespace

namespace SoaCa2StardewValley.Data;

/// <summary>
/// Reference data loaded through EF Core's HasData, so it is created by a migration and
/// anyone running the project gets the same crops and seasons.
/// Source: Stardew Valley Wiki, https://stardewvalleywiki.com/Crops
/// IMPORTANT: values are base prices written from memory. Check each against the wiki before you submit.
/// </summary>
public static class SeedData
{
    // Season ids
    private const int Spring = 1, Summer = 2, Fall = 3, Winter = 4;

    // Id, Name, SeedCost, GrowthDays, RegrowDays, SellPrice, YieldPerHarvest, season ids
    private static readonly (int Id, string Name, int Seed, int Growth, int? Regrow, int Sell, int Yield, int[] Seasons)[] CropRows =
    {
        (1,  "Parsnip",      20,  4,  null, 35,  1, new[] { Spring }),
        (2,  "Green Bean",   60,  10, 3,    40,  1, new[] { Spring }),
        (3,  "Cauliflower",  80,  12, null, 175, 1, new[] { Spring }),
        (4,  "Potato",       50,  6,  null, 80,  1, new[] { Spring }),
        (5,  "Kale",         70,  6,  null, 110, 1, new[] { Spring }),
        (6,  "Garlic",       40,  4,  null, 60,  1, new[] { Spring }),
        (7,  "Strawberry",   100, 8,  4,    120, 1, new[] { Spring }),
        (8,  "Rhubarb",      100, 13, null, 220, 1, new[] { Spring }),
        (9,  "Melon",        80,  12, null, 250, 1, new[] { Summer }),
        (10, "Tomato",       50,  11, 4,    60,  1, new[] { Summer }),
        (11, "Blueberry",    80,  13, 4,    50,  3, new[] { Summer }),
        (12, "Hot Pepper",   40,  5,  3,    40,  1, new[] { Summer }),
        (13, "Radish",       40,  6,  null, 90,  1, new[] { Summer }),
        (14, "Red Cabbage",  100, 9,  null, 260, 1, new[] { Summer }),
        (15, "Starfruit",    400, 13, null, 750, 1, new[] { Summer }),
        (16, "Corn",         150, 14, 4,    50,  1, new[] { Summer, Fall }),
        (17, "Wheat",        10,  4,  null, 25,  1, new[] { Summer, Fall }),
        (18, "Pumpkin",      100, 13, null, 320, 1, new[] { Fall }),
        (19, "Eggplant",     20,  5,  5,    60,  1, new[] { Fall }),
        (20, "Cranberries",  240, 7,  5,    75,  2, new[] { Fall }),
        (21, "Yam",          60,  10, null, 160, 1, new[] { Fall }),
        (22, "Bok Choy",     50,  4,  null, 80,  1, new[] { Fall }),
        (23, "Artichoke",    30,  8,  null, 160, 1, new[] { Fall }),
        (24, "Amaranth",     70,  7,  null, 150, 1, new[] { Fall }),
        (25, "Grape",        60,  10, 3,    80,  1, new[] { Fall }),
        (26, "Beet",         20,  6,  null, 100, 1, new[] { Fall }),
    };

    /// <summary>Call from AppDbContext.OnModelCreating after base.OnModelCreating.</summary>
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Season>().HasData(
            new Season { Id = Spring, Name = "Spring", LengthInDays = 28 },
            new Season { Id = Summer, Name = "Summer", LengthInDays = 28 },
            new Season { Id = Fall,   Name = "Fall",   LengthInDays = 28 },
            new Season { Id = Winter, Name = "Winter", LengthInDays = 28 });

        modelBuilder.Entity<Crop>().HasData(CropRows.Select(r => new Crop
        {
            Id = r.Id,
            Name = r.Name,
            SeedCost = r.Seed,
            GrowthDays = r.Growth,
            RegrowDays = r.Regrow,
            SellPrice = r.Sell,
            YieldPerHarvest = r.Yield
        }));

        modelBuilder.Entity<CropSeason>().HasData(
            CropRows.SelectMany(r => r.Seasons.Select(s => new CropSeason { CropId = r.Id, SeasonId = s })));
    }
}