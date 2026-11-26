using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SoaCa2StardewValley.Data;
using SoaCa2StardewValley.Dtos;

namespace SoaCa2StardewValley.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SeasonsController(AppDbContext db) => _db = db;

    // GET /api/seasons : all four seasons.
    [HttpGet]
    public async Task<ActionResult<List<SeasonDto>>> GetAll() =>
        await _db.Seasons
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new SeasonDto { Id = s.Id, Name = s.Name, LengthInDays = s.LengthInDays })
            .ToListAsync();

    // GET /api/seasons/2 : one season, or 404.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeasonDto>> GetById(int id)
    {
        var season = await _db.Seasons
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SeasonDto { Id = s.Id, Name = s.Name, LengthInDays = s.LengthInDays })
            .FirstOrDefaultAsync();

        return season is null ? NotFound() : Ok(season);
    }
}