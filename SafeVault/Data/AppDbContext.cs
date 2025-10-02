using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Secret> Secrets { get; set; }
}
