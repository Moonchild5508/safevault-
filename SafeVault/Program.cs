using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafeVault.Data;
using SafeVault.Services;

var builder = WebApplication.CreateBuilder(args);

// In-memory DB for demo purposes
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("SafeVaultDb"));
builder.Services.AddScoped<AuthService>();

// JWT Authentication (demo key - DO NOT use in production)
var key = Encoding.ASCII.GetBytes("SuperSecretDemoKey_ChangeThis!");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddControllers();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Users.Add(new Models.User { Id = 1, Username = "admin", Role = "Admin", PasswordHash = "adminpass" });
    db.Users.Add(new Models.User { Id = 2, Username = "alice", Role = "User", PasswordHash = "alicepass" });
    db.Secrets.Add(new Models.Secret { Id = 1, OwnerId = 1, Content = "Top Secret Admin Data" });
    db.Secrets.Add(new Models.Secret { Id = 2, OwnerId = 2, Content = "<script>alert('xss')</script> Public secret" });
    db.SaveChanges();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
