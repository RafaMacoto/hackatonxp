using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DbContext: use InMemory by default to simplify local execution in the hackathon.
// If a PostgreSQL connection string is provided in configuration under "ConnectionStrings:Postgres",
// the app will use Npgsql instead.
var postgres = builder.Configuration.GetConnectionString("Postgres");
var allowInMemory = builder.Configuration["HACKATONXP_ALLOW_INMEMORY"] ?? Environment.GetEnvironmentVariable("HACKATONXP_ALLOW_INMEMORY");
if (!string.IsNullOrWhiteSpace(postgres))
{
    builder.Services.AddDbContext<Advisor360.Infrastructure.Data.Advisor360DbContext>(options =>
        options.UseNpgsql(postgres));
}
else if (string.Equals(allowInMemory, "true", StringComparison.OrdinalIgnoreCase))
{
    // Explicit fallback for development when developer opts-in via env var
    builder.Services.AddDbContext<Advisor360.Infrastructure.Data.Advisor360DbContext>(options =>
        options.UseInMemoryDatabase("Advisor360_InMemory"));
}
else
{
    // Fail fast with clear guidance if Postgres is not configured and in-memory is not allowed
    throw new InvalidOperationException("PostgreSQL connection string not configured. Set ConnectionStrings__Postgres or set HACKATONXP_ALLOW_INMEMORY=true for local ephemeral execution. See README.md.");
}

// Application dependencies
builder.Services.AddScoped<Advisor360.Infrastructure.Repositories.IClientRepository, Advisor360.Infrastructure.Repositories.ClientRepository>();
builder.Services.AddScoped<Advisor360.Application.Interfaces.IClientOverviewService, Advisor360.Application.Services.ClientOverviewService>();
builder.Services.AddScoped<Advisor360.Application.Interfaces.ICpfService, Advisor360.Application.Services.CpfService>();
builder.Services.AddScoped<Advisor360.Application.Interfaces.IClientWriteService, Advisor360.Application.Services.ClientWriteService>();

var app = builder.Build();

// Ensure seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Advisor360.Infrastructure.Data.Advisor360DbContext>();
    db.Database.EnsureCreated();
    Advisor360.Infrastructure.Seed.DataSeeder.EnsureSeedData(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
