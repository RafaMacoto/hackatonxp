using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Infrastructure.Repositories;

namespace hackatonxp.Application.Services
{
    public class ClientOverviewService : IClientOverviewService
    {
        private readonly IClientRepository _clientRepository;

        public ClientOverviewService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientOverviewDto?> GetOverviewAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdWithRelatedAsync(clientId, cancellationToken);
            if (client is null) return null;

            var positions = client.Positions.Select(p => new PositionDto
            {
                AssetName = p.AssetName,
                AssetType = p.AssetType.ToString(),
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                ReferenceDate = p.ReferenceDate,
                MarketValue = p.Quantity * p.UnitPrice
            }).ToList();

            var portfolioTotal = positions.Sum(p => p.MarketValue);

            var recentInteractions = client.Interactions
                .OrderByDescending(i => i.Date)
                .Select(i => new InteractionDto { Date = i.Date, Channel = i.Channel.ToString(), Summary = i.Summary })
                .ToList();

            var openTasks = client.Tasks
                .Where(t => t.Status != hackatonxp.Domain.Entities.TaskItemStatus.Completed)
                .OrderBy(t => t.DueDate)
                .Select(t => new TaskDto { Id = t.Id, Title = t.Title, DueDate = t.DueDate, Status = t.Status.ToString() })
                .ToList();

            var upcomingDates = client.ImportantDates
                .Where(d => d.Date >= DateTime.UtcNow.Date)
                .OrderBy(d => d.Date)
                .Select(d => new ImportantDateDto { Title = d.Title, Date = d.Date, Category = d.Category.ToString(), Notes = d.Notes })
                .ToList();

            return new ClientOverviewDto
            {
                Client = new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Phone = client.Phone,
                    BirthDate = client.BirthDate,
                    InvestorProfile = client.InvestorProfile?.ToString()
                },
                PortfolioTotal = portfolioTotal,
                Positions = positions,
                RecentInteractions = recentInteractions,
                OpenTasks = openTasks,
                UpcomingDates = upcomingDates
            };
        }
    }
}
