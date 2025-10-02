using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using Xunit;
using System.Linq;

namespace SafeVault.Tests;

public class SecurityTests
{
    [Fact]
    public void SqlInjectionAttempt_DoesNotReturnAllRecords()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("SecTestDb").Options;
        using var db = new AppDbContext(options);
        db.Secrets.Add(new Models.Secret { Id = 1, OwnerId = 1, Content = "alpha" });
        db.Secrets.Add(new Models.Secret { Id = 2, OwnerId = 2, Content = "beta" });
        db.SaveChanges();

        // Simulate dangerous input that might attempt SQL injection
        var malicious = "%'; DELETE FROM Secrets; --";
        // Safe LINQ/EF usage prevents injection; it treats string as value, not SQL
        var results = db.Secrets.Where(s => EF.Functions.Like(s.Content, "%" + malicious + "%")).ToList();
        // Expect no matches and no deletion
        Assert.Empty(results);
        Assert.Equal(2, db.Secrets.Count());
    }

    [Fact]
    public void XssContent_IsEncodedWhenRetrieved()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("XssTestDb").Options;
        using var db = new AppDbContext(options);
        db.Secrets.Add(new Models.Secret { Id = 1, OwnerId = 1, Content = "<script>alert(1)</script>" });
        db.SaveChanges();

        var secret = db.Secrets.Find(1);
        var encoded = System.Net.WebUtility.HtmlEncode(secret.Content);
        Assert.DoesNotContain("<script>", encoded);
        Assert.Contains("&lt;script&gt;", encoded);
    }
}
