namespace SoaCa2StardewValley.Models;

public class Season
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int LengthInDays { get; set; } = 28;

    public ICollection<CropSeason> CropSeasons { get; set; } = new List<CropSeason>();
}