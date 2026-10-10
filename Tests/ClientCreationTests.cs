using System;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;
using Advisor360.Application.Services;
using Advisor360.Infrastructure.Data;
using Advisor360.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Advisor360.Tests
{
    public class ClientCreationTests
    {
        private Advisor360DbContext CreateDb()
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
        public async Task CreateClient_DuplicateCpf_Throws()
        {
            using var db = CreateDb();
            var repo = new ClientRepository(db);
            var cpfService = new Advisor360.Application.Services.CpfService();
            var overviewService = new Advisor360.Application.Services.ClientOverviewService(repo);
            var writeService = new Advisor360.Application.Services.ClientWriteService(repo, cpfService, overviewService);

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
