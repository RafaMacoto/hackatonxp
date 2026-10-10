using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Advisor360.Infrastructure.Data
{
    // Design-time factory to support 'dotnet ef' tooling without embedding credentials.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Advisor360DbContext>
    {
        public Advisor360DbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Advisor360DbContext>();

            // Try to read connection string from environment variable; tooling should set it when needed.
            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres");
            if (string.IsNullOrEmpty(cs))
            {
                // Fallback: use an in-memory provider for design-time when Postgres is not configured.
                builder.UseInMemoryDatabase("Advisor360_DesignTime");
            }
            else
            {
                builder.UseNpgsql(cs);
            }

            return new Advisor360DbContext(builder.Options);
        }
    }
}
