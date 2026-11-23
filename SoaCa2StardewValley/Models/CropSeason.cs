namespace SoaCa2StardewValley.Models;

public class CropSeason
{
    public int CropId { get; set; }
    public Crop Crop { get; set; } = null!;

    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;
}