using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SafeVault.Data;

namespace SafeVault.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly byte[] _key = Encoding.ASCII.GetBytes("SuperSecretDemoKey_ChangeThis!"); // demo only

    public AuthService(AppDbContext db) { _db = db; }

    // Very simple authentication for demo. In production, verify hashed passwords.
    public string? Authenticate(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        if (user == null) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
