using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrolmentController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EnrolmentController(RaceDayContext context) => _context = context;

    public sealed class CreateEnrolmentRequest
    {
        public int EventID { get; set; }
        public int CategoryID { get; set; }
    }

    [HttpGet]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments() =>
        Ok(await _context.Enrolments.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
    {
        var item = await _context.Enrolments.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<ActionResult<Enrolment>> CreateEnrolment(CreateEnrolmentRequest request)
    {
        var idText = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idText, out var participantId)) return Unauthorized();
        var item = new Enrolment
        {
            ParticipantID = participantId,
            EventID = request.EventID,
            CategoryID = request.CategoryID,
            EnrolmentDate = DateTime.UtcNow
        };
        _context.Enrolments.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEnrolment), new { id = item.EnrolmentID }, item);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEnrolment(int id, Enrolment update)
    {
        var item = await _context.Enrolments.FindAsync(id);
        if (item is null) return NotFound();
        item.ParticipantID = update.ParticipantID;
        item.EventID = update.EventID;
        item.CategoryID = update.CategoryID;
        item.EnrolmentDate = update.EnrolmentDate;
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEnrolment(int id)
    {
        var item = await _context.Enrolments.FindAsync(id);
        if (item is null) return NotFound();
        _context.Enrolments.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
