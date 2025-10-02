using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using SafeVault.Services;
using Xunit;

namespace SafeVault.Tests;

public class AuthTests
{
    [Fact]
    public void Authenticate_ValidCredentials_ReturnsToken()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("AuthTestDb").Options;
        using var db = new AppDbContext(options);
        db.Users.Add(new Models.User { Id = 1, Username = "bob", PasswordHash = "bobpass", Role = "User" });
        db.SaveChanges();

        var svc = new AuthService(db);
        var token = svc.Authenticate("bob", "bobpass");
        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public void Authenticate_InvalidCredentials_ReturnsNull()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("AuthTestDb2").Options;
        using var db = new AppDbContext(options);
        var svc = new AuthService(db);
        var token = svc.Authenticate("nope", "wrong");
        Assert.Null(token);
    }
}
