using hackatonxp.Application.Interfaces;
using hackatonxp.Infrastructure.Repositories;
using hackatonxp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "hackatonxp API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cole seu token JWT aqui."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});



// Configuração do banco de dados
var postgres = builder.Configuration.GetConnectionString("Postgres");

var allowInMemory =
    builder.Configuration["HACKATONXP_ALLOW_INMEMORY"]
    ?? Environment.GetEnvironmentVariable("HACKATONXP_ALLOW_INMEMORY");

if (!string.IsNullOrWhiteSpace(postgres))
{
    builder.Services.AddDbContext<
        hackatonxp.Infrastructure.Data.HackatonXpDbContext>(
        options => options.UseNpgsql(postgres));
}
else if (string.Equals(
    allowInMemory,
    "true",
    StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<
        hackatonxp.Infrastructure.Data.HackatonXpDbContext>(
        options => options.UseInMemoryDatabase("HackatonXp_InMemory"));
}
else
{
    throw new InvalidOperationException(
        "Connection string do PostgreSQL não configurada. " +
        "Configure ConnectionStrings__Postgres ou " +
        "HACKATONXP_ALLOW_INMEMORY=true para desenvolvimento local.");
}

// Serviços de clientes
builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IClientRepository,
    hackatonxp.Infrastructure.Repositories.ClientRepository>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IClientService,
    hackatonxp.Application.Services.ClientService>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IClientOverviewService,
    hackatonxp.Application.Services.ClientOverviewService>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.ICpfService,
    hackatonxp.Application.Services.CpfService>();

// Integração com a Pluggy
builder.Services.AddHttpClient<IPluggyService, PluggyService>(client =>
{
    client.BaseAddress = new Uri("https://api.pluggy.ai/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Repositórios e serviços de assessores
builder.Services.AddScoped<
    hackatonxp.Infrastructure.Repositories.IAdvisorRepository,
    hackatonxp.Infrastructure.Repositories.AdvisorRepository>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IPasswordHasher,
    hackatonxp.Infrastructure.Services.PasswordHasherService>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IAdvisorService,
    hackatonxp.Application.Services.AdvisorService>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IAuthService,
    hackatonxp.Application.Services.AuthService>();

builder.Services.AddScoped<
    hackatonxp.Application.Interfaces.IJwtService,
    hackatonxp.Infrastructure.Services.JwtService>();

// Configuração JWT
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? Environment.GetEnvironmentVariable("JWT_SECRET");

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException(
        "JWT não configurado. Defina a variável JWT_SECRET.");
}

if (Encoding.UTF8.GetByteCount(jwtSecret) < 32)
{
    throw new InvalidOperationException(
        "JWT_SECRET deve ter pelo menos 32 bytes.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "hackatonxp";
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? "hackatonxp_clients";

var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSecret));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Inicialização do banco e assessor de demonstração
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<
        hackatonxp.Infrastructure.Data.HackatonXpDbContext>();

    if (db.Database.IsRelational())
    {
        Console.WriteLine(
            "Banco relacional configurado. Verificando migrations...");

        // Não cria tabelas automaticamente no PostgreSQL.
        // Execute dotnet ef database update antes de iniciar, se necessário.
    }
    else
    {
        db.Database.EnsureCreated();

        hackatonxp.Infrastructure.Seed.DataSeeder
            .EnsureSeedData(db);
    }

    if (app.Environment.IsDevelopment())
    {
        var adminEmail =
            Environment.GetEnvironmentVariable("HACKATONXP_ADMIN_EMAIL")
            ?? "admin@hackatonxp.local";

        var adminPassword =
            Environment.GetEnvironmentVariable("HACKATONXP_ADMIN_PASSWORD")
            ?? "HackatonXp@123";

        var advisorRepo = scope.ServiceProvider
            .GetRequiredService<
                hackatonxp.Infrastructure.Repositories.IAdvisorRepository>();

        var hasher = scope.ServiceProvider
            .GetRequiredService<
                hackatonxp.Application.Interfaces.IPasswordHasher>();

        var existing = await advisorRepo.GetByEmailAsync(adminEmail);

        if (existing is null)
        {
            var advisor = new hackatonxp.Domain.Entities.Advisor
            {
                Id = Guid.NewGuid(),
                Name = "Assessor Demo",
                Email = adminEmail,
                PasswordHash = hasher.HashPassword(adminPassword),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await advisorRepo.AddAsync(advisor);

            Console.WriteLine(
                $"Assessor de demonstração criado: {adminEmail}");
        }
        else
        {
            // Mantém credenciais conhecidas no ambiente de desenvolvimento.
            existing.Name = "Assessor Demo";
            existing.PasswordHash = hasher.HashPassword(adminPassword);
            existing.IsActive = true;

            await db.SaveChangesAsync();

            Console.WriteLine(
                $"Assessor de demonstração atualizado: {adminEmail}");
        }
    }
}

// Pipeline HTTP
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