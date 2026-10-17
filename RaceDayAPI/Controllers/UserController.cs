using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Organiser")]
public class UserController : ControllerBase
{
    private readonly RaceDayContext _context;
    private readonly PasswordHasher<User> _hasher = new();
    public UserController(RaceDayContext context) => _context = context;

    public sealed class CreateUserRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Phone { get; set; }
    }

    public sealed class UpdateUserRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
    }

    private static object ToSafeUser(User user) => new
    {
        user.UserID, user.FirstName, user.LastName, user.Email, user.Role, user.Phone
    };

    [HttpGet]
    public async Task<IActionResult> GetUsers() =>
        Ok((await _context.Users.AsNoTracking().OrderBy(u => u.UserID).ToListAsync()).Select(ToSafeUser));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(ToSafeUser(user));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and password are required.");
        if (await _context.Users.AnyAsync(u => u.Email == email))
            return Conflict("An account with this email already exists.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = email,
            Role = "Participant",
            Phone = request.Phone
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, ToSafeUser(user));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest update)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        var email = update.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.UserID != id && u.Email == email))
            return Conflict("An account with this email already exists.");
        user.FirstName = update.FirstName;
        user.LastName = update.LastName;
        user.Email = email;
        user.Phone = update.Phone;
        await _context.SaveChangesAsync();
        return Ok(ToSafeUser(user));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
