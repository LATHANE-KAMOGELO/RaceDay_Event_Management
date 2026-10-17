using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultController : ControllerBase
{
    private readonly RaceDayContext _context;
    public ResultController(RaceDayContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<IEnumerable<Result>>> GetResults() =>
        Ok(await _context.Results.AsNoTracking().OrderBy(r => r.FinishPosition).ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<Result>> GetResult(int id)
    {
        var item = await _context.Results.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<Result>> CreateResult(Result item)
    {
        _context.Results.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetResult), new { id = item.ResultID }, item);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateResult(int id, Result update)
    {
        var item = await _context.Results.FindAsync(id);
        if (item is null) return NotFound();
        item.EnrolmentID = update.EnrolmentID;
        item.FinishTime = update.FinishTime;
        item.FinishPosition = update.FinishPosition;
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        var item = await _context.Results.FindAsync(id);
        if (item is null) return NotFound();
        _context.Results.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
