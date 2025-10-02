namespace SafeVault.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    // NOTE: For demo only. Store hashed & salted passwords in production.
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
