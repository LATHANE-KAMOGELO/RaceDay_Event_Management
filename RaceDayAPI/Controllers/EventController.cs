using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EventController(RaceDayContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents() =>
        Ok(await _context.Events.AsNoTracking().OrderBy(e => e.EventDate).ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var item = await _context.Events.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<Event>> CreateEvent(Event item)
    {
        _context.Events.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = item.EventID }, item);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(int id, Event update)
    {
        var item = await _context.Events.FindAsync(id);
        if (item is null) return NotFound();
        item.OrganiserId = update.OrganiserId;
        item.Name = update.Name;
        item.Description = update.Description;
        item.EventDate = update.EventDate;
        item.Location = update.Location;
        item.Distance = update.Distance;
        item.EventType = update.EventType;
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var item = await _context.Events.FindAsync(id);
        if (item is null) return NotFound();
        _context.Events.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
