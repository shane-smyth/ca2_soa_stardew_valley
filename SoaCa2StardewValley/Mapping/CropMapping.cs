using SoaCa2StardewValley.Dtos;
using SoaCa2StardewValley.Models;

namespace SoaCa2StardewValley.Mapping;

public static class CropMapping
{
    public static CropDto ToDto(this Crop crop) => new()
    {
        Id = crop.Id,
        Name = crop.Name,
        SeedCost = crop.SeedCost,
        GrowthDays = crop.GrowthDays,
        RegrowDays = crop.RegrowDays,
        SellPrice = crop.SellPrice,
        YieldPerHarvest = crop.YieldPerHarvest,
        Seasons = crop.CropSeasons.Select(cs => cs.Season.Name).OrderBy(n => n).ToList()
    };

    public static Crop ToEntity(this CropWriteDto dto)
    {
        var crop = new Crop { Name = dto.Name.Trim() };
        dto.ApplyTo(crop);
        return crop;
    }

    // Copies the DTO's values onto an existing (tracked) entity, replacing its seasons.
    public static void ApplyTo(this CropWriteDto dto, Crop crop)
    {
        crop.Name = dto.Name.Trim();
        crop.SeedCost = dto.SeedCost;
        crop.GrowthDays = dto.GrowthDays;
        crop.RegrowDays = dto.RegrowDays;
        crop.SellPrice = dto.SellPrice;
        crop.YieldPerHarvest = dto.YieldPerHarvest;

        crop.CropSeasons.Clear();
        foreach (var seasonId in dto.SeasonIds.Distinct())
        {
            crop.CropSeasons.Add(new CropSeason { SeasonId = seasonId });
        }
    }
}