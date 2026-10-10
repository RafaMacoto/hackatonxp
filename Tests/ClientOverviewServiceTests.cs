using System;
using System.Linq;
using System.Threading.Tasks;
using Advisor360.Application.Services;
using Advisor360.Infrastructure.Data;
using Advisor360.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Advisor360.Tests
{
    public class ClientOverviewServiceTests
    {
        private Advisor360DbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<Advisor360DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var db = new Advisor360DbContext(options);
            db.Database.EnsureCreated();
            Advisor360.Infrastructure.Seed.DataSeeder.EnsureSeedData(db);
            return db;
        }

        [Fact]
        public async Task GetOverview_ClientExists_ReturnsOverview()
        {
            using var db = CreateInMemoryDb();
            var repo = new ClientRepository(db);
            var service = new ClientOverviewService(repo);
            var clientId = db.Clients.Select(c => c.Id).First();

            var overview = await service.GetOverviewAsync(clientId);

            Assert.NotNull(overview);
            Assert.Equal(clientId, overview.Client.Id);
        }

        [Fact]
        public async Task GetOverview_ClientNotFound_ReturnsNull()
        {
            using var db = CreateInMemoryDb();
            var repo = new ClientRepository(db);
            var service = new ClientOverviewService(repo);

            var overview = await service.GetOverviewAsync(Guid.NewGuid());

            Assert.Null(overview);
        }

        [Fact]
        public async Task GetOverview_CalculatesPortfolioTotalCorrectly()
        {
            using var db = CreateInMemoryDb();
            var repo = new ClientRepository(db);
            var service = new ClientOverviewService(repo);
            var client = db.Clients.Include(c => c.Positions).First();

            var overview = await service.GetOverviewAsync(client.Id);

            var expected = client.Positions.Sum(p => p.Quantity * p.UnitPrice);
            Assert.Equal(expected, overview.PortfolioTotal);
        }
    }
}
