using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteController : ControllerBase
{
    private readonly RaceDayContext _context;
    public RouteController(RaceDayContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<IEnumerable<RaceDayAPI.Models.Route>>> GetRoutes() =>
        Ok(await _context.Routes.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<RaceDayAPI.Models.Route>> GetRoute(int id)
    {
        var item = await _context.Routes.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<RaceDayAPI.Models.Route>> CreateRoute(RaceDayAPI.Models.Route item)
    {
        _context.Routes.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoute), new { id = item.RouteID }, item);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateRoute(int id, RaceDayAPI.Models.Route update)
    {
        var item = await _context.Routes.FindAsync(id);
        if (item is null) return NotFound();
        item.EventID = update.EventID;
        item.RouteName = update.RouteName;
        item.StartLocation = update.StartLocation;
        item.FinishLocation = update.FinishLocation;
        item.Distance = update.Distance;
        item.RouteDescription = update.RouteDescription;
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        var item = await _context.Routes.FindAsync(id);
        if (item is null) return NotFound();
        _context.Routes.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
