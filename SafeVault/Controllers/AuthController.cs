using Microsoft.AspNetCore.Mvc;
using SafeVault.Services;
using SafeVault.Models;
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var token = _auth.Authenticate(req.Username.Trim(), req.Password);
        if (token == null) return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { token });
    }
}
