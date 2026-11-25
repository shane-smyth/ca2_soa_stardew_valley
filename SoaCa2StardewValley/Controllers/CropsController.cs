using Microsoft.AspNetCore.Mvc;
using SoaCa2StardewValley.Dtos;
using SoaCa2StardewValley.Data;
using SoaCa2StardewValley.Mapping;
using Microsoft.EntityFrameworkCore;

namespace SoaCa2StardewValley.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CropsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CropsController(AppDbContext db) => _db = db;

    // GET /api/crops?seasonId=1 : list crops, optionally filtered by season.
    [HttpGet]
    public async Task<ActionResult<List<CropDto>>> GetAll([FromQuery] int? seasonId)
    {
        var query = _db.Crops
            .AsNoTracking() // read-only query, no change tracking needed
            .Include(c => c.CropSeasons).ThenInclude(cs => cs.Season)
            .AsQueryable();

        if (seasonId.HasValue)
        {
            query = query.Where(c => c.CropSeasons.Any(cs => cs.SeasonId == seasonId.Value));
        }

        var crops = await query.OrderBy(c => c.Name).ToListAsync();
        return Ok(crops.Select(c => c.ToDto()).ToList());
    }

    // GET /api/crops/5 : one crop, or 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CropDto>> GetById(int id)
    {
        var crop = await LoadCropAsync(id);
        return crop is null ? NotFound() : Ok(crop.ToDto());
    }

    // POST /api/crops : create. Not idempotent: each call creates a new row. Returns 201.
    [HttpPost]
    public async Task<ActionResult<CropDto>> Create(CropWriteDto dto)
    {
        if (await NameExistsAsync(dto.Name))
            return Conflict(new { message = $"A crop named '{dto.Name}' already exists." });

        if (!await AllSeasonsExistAsync(dto.SeasonIds))
            return BadRequest(new { message = "One or more SeasonIds do not exist." });

        var crop = dto.ToEntity();
        _db.Crops.Add(crop);
        await _db.SaveChangesAsync();

        // Reload so the response includes the season names.
        var created = await LoadCropAsync(crop.Id);
        return CreatedAtAction(nameof(GetById), new { id = crop.Id }, created!.ToDto());
    }

    // PUT /api/crops/5 : replace a crop. Idempotent: repeating the same call gives the same result
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CropWriteDto dto)
    {
        var crop = await LoadCropAsync(id);
        if (crop is null) return NotFound();

        if (await NameExistsAsync(dto.Name, excludeId: id))
            return Conflict(new { message = $"A crop named '{dto.Name}' already exists." });

        if (!await AllSeasonsExistAsync(dto.SeasonIds))
            return BadRequest(new { message = "One or more SeasonIds do not exist." });

        dto.ApplyTo(crop);
        await _db.SaveChangesAsync();
        return NoContent(); // 204
    }

    // DELETE /api/crops/5 : blocked with 409 if saved plans use the crop
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var crop = await LoadCropAsync(id);
        if (crop is null) return NotFound();

        if (await _db.PlanEntries.AnyAsync(pe => pe.CropId == id))
            return Conflict(new { message = "This crop is used by saved plans and cannot be deleted." });

        _db.Crops.Remove(crop);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---- private helpers ----

    // Loads a tracked crop with its seasons, or null
    private Task<Models.Crop?> LoadCropAsync(int id) =>
        _db.Crops
            .Include(c => c.CropSeasons).ThenInclude(cs => cs.Season)
            .FirstOrDefaultAsync(c => c.Id == id);

    private Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        var normalised = name.Trim().ToLower();
        return _db.Crops.AnyAsync(c => c.Name.ToLower() == normalised && c.Id != excludeId);
    }

    private async Task<bool> AllSeasonsExistAsync(IEnumerable<int> seasonIds)
    {
        var ids = seasonIds.Distinct().ToList();
        var found = await _db.Seasons.CountAsync(s => ids.Contains(s.Id));
        return found == ids.Count;
    }
}