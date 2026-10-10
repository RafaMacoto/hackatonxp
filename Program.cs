using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using hackatonxp.Application.Interfaces;
using hackatonxp.Infrastructure.Repositories;
using hackatonxp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext: use InMemory by default to simplify local execution in the hackathon.
// If a PostgreSQL connection string is provided in configuration under "ConnectionStrings:Postgres",
// the app will use Npgsql instead.
var postgres = builder.Configuration.GetConnectionString("Postgres");
var allowInMemory = builder.Configuration["HACKATONXP_ALLOW_INMEMORY"] ?? Environment.GetEnvironmentVariable("HACKATONXP_ALLOW_INMEMORY");
if (!string.IsNullOrWhiteSpace(postgres))
{
    builder.Services.AddDbContext<hackatonxp.Infrastructure.Data.HackatonXpDbContext>(options =>
        options.UseNpgsql(postgres));
}
else if (string.Equals(allowInMemory, "true", StringComparison.OrdinalIgnoreCase))
{
    // Explicit fallback for development when developer opts-in via env var
    builder.Services.AddDbContext<hackatonxp.Infrastructure.Data.HackatonXpDbContext>(options =>
        options.UseInMemoryDatabase("HackatonXp_InMemory"));
}
else
{
    // Fail fast with clear guidance if Postgres is not configured and in-memory is not allowed
    throw new InvalidOperationException("PostgreSQL connection string not configured. Set ConnectionStrings__Postgres or set HACKATONXP_ALLOW_INMEMORY=true for local ephemeral execution. See README.md.");
}

// Application dependencies
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IClientRepository, hackatonxp.Infrastructure.Repositories.ClientRepository>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IClientService, hackatonxp.Application.Services.ClientService>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IClientOverviewService, hackatonxp.Application.Services.ClientOverviewService>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.ICpfService, hackatonxp.Application.Services.CpfService>();

// Advisor auth services
builder.Services.AddScoped<hackatonxp.Infrastructure.Repositories.IAdvisorRepository, hackatonxp.Infrastructure.Repositories.AdvisorRepository>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IPasswordHasher, hackatonxp.Infrastructure.Services.PasswordHasherService>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IAdvisorService, hackatonxp.Application.Services.AdvisorService>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IAuthService, hackatonxp.Application.Services.AuthService>();
builder.Services.AddScoped<hackatonxp.Application.Interfaces.IJwtService, hackatonxp.Infrastructure.Services.JwtService>();

// Configure JWT authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");
if (!string.IsNullOrEmpty(jwtSecret))
{
    var key = Encoding.UTF8.GetBytes(jwtSecret);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "hackatonxp",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "hackatonxp_clients",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}
else
{
    // No JWT secret configured; warn developer. Authentication endpoints will throw if attempted.
    Console.WriteLine("Warning: JWT secret not configured. Set Jwt:Secret or JWT_SECRET to enable authentication.");
}

var app = builder.Build();

// Ensure seed data
using (var scope = app.Services.CreateScope())
{
    // Seed only when using in-memory provider (explicit opt-in). For PostgreSQL, apply migrations via EF CLI and then run seed manually if desired.
    var db = scope.ServiceProvider.GetRequiredService<hackatonxp.Infrastructure.Data.HackatonXpDbContext>();
    var provider = db.Database.ProviderName;
    if (provider?.Contains("InMemory") == true)
    {
        db.Database.EnsureCreated();
        hackatonxp.Infrastructure.Seed.DataSeeder.EnsureSeedData(db);
    }
    else
    {
        // Do not apply migrations or ensure database here; instruct developers to run migrations explicitly.
        Console.WriteLine("PostgreSQL provider configured. Ensure migrations are applied with 'dotnet ef database update' before seeding.");
    }

    // Optional: create initial advisor when environment variables are provided (controlled initialization)
    var createAdmin = Environment.GetEnvironmentVariable("HACKATONXP_CREATE_ADMIN");
    var adminEmail = Environment.GetEnvironmentVariable("HACKATONXP_ADMIN_EMAIL");
    var adminPassword = Environment.GetEnvironmentVariable("HACKATONXP_ADMIN_PASSWORD");
    if (string.Equals(createAdmin, "true", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
    {
        var advisorRepo = scope.ServiceProvider.GetRequiredService<hackatonxp.Infrastructure.Repositories.IAdvisorRepository>();
        var hasher = scope.ServiceProvider.GetRequiredService<hackatonxp.Application.Interfaces.IPasswordHasher>();
        var existing = advisorRepo.GetByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (existing is null)
        {
            var advisor = new hackatonxp.Domain.Entities.Advisor
            {
                Id = Guid.NewGuid(),
                Name = "Initial Admin",
                Email = adminEmail,
                PasswordHash = hasher.HashPassword(adminPassword),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            advisorRepo.AddAsync(advisor).GetAwaiter().GetResult();
            Console.WriteLine("Initial advisor created: " + adminEmail);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
