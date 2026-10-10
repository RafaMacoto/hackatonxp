using System;
using System.Linq;
using Advisor360.Domain.Entities;
using Advisor360.Infrastructure.Data;

namespace Advisor360.Infrastructure.Seed
{
    public static class DataSeeder
    {
        public static void EnsureSeedData(Advisor360DbContext db)
        {
            if (db.Clients.Any())
                return;

            var clientId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var client = new Client
            {
                Id = clientId,
                Cpf = "52998224725",
                Name = "Cliente Demo",
                Email = "cliente.demo@example.com",
                Phone = "+55 11 99999-0000",
                BirthDate = new DateTime(1985, 6, 15),
                InvestorProfile = InvestorProfile.Balanced,
                FinancialGoals = "Formar reserva e investir para aposentadoria",
                CommunicationPreferences = "Email",
                CreatedAt = DateTime.UtcNow
            };

            client.Positions.Add(new PortfolioPosition
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                AssetName = "Fundo A",
                AssetType = AssetType.Fund,
                Quantity = 100m,
                UnitPrice = 12.34m,
                ReferenceDate = DateTime.UtcNow.Date
            });

            client.Positions.Add(new PortfolioPosition
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                AssetName = "Acao XYZ",
                AssetType = AssetType.Stock,
                Quantity = 50m,
                UnitPrice = 45.67m,
                ReferenceDate = DateTime.UtcNow.Date
            });

            client.Interactions.Add(new Interaction
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Date = DateTime.UtcNow.AddDays(-2),
                Channel = InteractionChannel.Meeting,
                Summary = "Reunião inicial - alinhamento de objetivos",
                Topics = "Perfil, objetivos",
                NextSteps = "Enviar proposta"
            });

            client.Interactions.Add(new Interaction
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Date = DateTime.UtcNow.AddDays(-10),
                Channel = InteractionChannel.Email,
                Summary = "Envio de documentos",
                Topics = "Documentos",
                NextSteps = null
            });

            client.Tasks.Add(new TaskItem
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Title = "Enviar declaração",
                Description = "Pedir documentação complementar",
                DueDate = DateTime.UtcNow.AddDays(7),
                Status = Domain.Entities.TaskItemStatus.Open,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            });

            client.ImportantDates.Add(new ImportantDate
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Title = "Aniversário",
                Date = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(30),
                Category = ImportantDateCategory.Birthday,
                Notes = "Enviar felicitações"
            });

            db.Clients.Add(client);
            db.SaveChanges();
        }
    }
}
