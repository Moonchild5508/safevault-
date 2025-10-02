using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using SafeVault.Utils;

namespace SafeVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecretsController : ControllerBase
{
    private readonly AppDbContext _db;
    public SecretsController(AppDbContext db) => _db = db;

    // Get secret by id - only owner or Admin
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        // Validate input (defense-in-depth)
        if (!InputValidator.IsValidId(id)) return BadRequest(new { message = "Invalid id" });

        var secret = await _db.Secrets.FindAsync(id);
        if (secret == null) return NotFound();

        // Authorization
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "User";
        if (role != "Admin" && userIdClaim != secret.OwnerId.ToString())
            return Forbid();

        // Sanitize output to prevent reflected XSS if returned to a browser
        var safeContent = System.Net.WebUtility.HtmlEncode(secret.Content);
        return Ok(new { id = secret.Id, content = safeContent });
    }

    // Search secrets by content (demonstrates parameterized LINQ - prevents SQL injection)
    [HttpGet("search")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return BadRequest(new { message = "Query required" });

        // Input normalization
        q = q.Trim();

        // Use EF Core LINQ - not string concatenation - safe against SQL injection
        var results = await _db.Secrets
            .Where(s => EF.Functions.Like(s.Content, "%" + q + "%"))
            .Select(s => new { s.Id, content = System.Net.WebUtility.HtmlEncode(s.Content) })
            .ToListAsync();

        return Ok(results);
    }
}
