using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly RaceDayContext _context;
    public CategoryController(RaceDayContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories() =>
        Ok(await _context.Categories.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Participant,Organiser")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category is null ? NotFound() : Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryID }, category);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateCategory(int id, Category update)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return NotFound();
        category.EventID = update.EventID;
        category.CategoryName = update.CategoryName;
        category.AgeMinimum = update.AgeMinimum;
        category.AgeMaximum = update.AgeMaximum;
        category.Distance = update.Distance;
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return NotFound();
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
