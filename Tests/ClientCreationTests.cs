using System;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Services;
using hackatonxp.Infrastructure.Data;
using hackatonxp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace hackatonxp.UnitTests
{
    public class ClientCreationTests
    {
        private HackatonXpDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<HackatonXpDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var db = new HackatonXpDbContext(options);
            db.Database.EnsureCreated();
            hackatonxp.Infrastructure.Seed.DataSeeder.EnsureSeedData(db);
            return db;
        }

        [Fact]
        public async Task CreateClient_DuplicateCpf_Throws()
        {
            using var db = CreateDb();
            var repo = new ClientRepository(db);
            var cpfService = new hackatonxp.Application.Services.CpfService();
            var overviewService = new hackatonxp.Application.Services.ClientOverviewService(repo);
            var writeService = new hackatonxp.Application.Services.ClientWriteService(repo, cpfService, overviewService);

            var dto = new ClientCreateDto
            {
                Name = "Novo Cliente",
                Cpf = "529.982.247-25",
                Email = "novo@example.com"
            };

            // Existing seed includes a client with CPF 52998224725
            await Assert.ThrowsAsync<InvalidOperationException>(() => writeService.CreateAsync(dto));
        }
    }
}
