using System;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace hackatonxp.Infrastructure.Data
{
    // Design-time factory to support 'dotnet ef' tooling. Requires ConnectionStrings__Postgres to be set in environment.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HackatonXpDbContext>
    {
        public HackatonXpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HackatonXpDbContext>();

            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres");
            if (string.IsNullOrEmpty(cs))
            {
                throw new InvalidOperationException("ConnectionStrings__Postgres is not set. Set the environment variable to enable design-time DbContext creation for PostgreSQL.");
            }

            builder.UseNpgsql(cs);

            return new HackatonXpDbContext(builder.Options);
        }
    }
}
