using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;
using Advisor360.Application.Interfaces;
using Advisor360.Domain.Entities;
using Advisor360.Infrastructure.Repositories;

namespace Advisor360.Application.Services
{
    public class ClientWriteService : IClientWriteService
    {
        private readonly IClientRepository _repository;
        private readonly ICpfService _cpfService;
        private readonly IClientOverviewService _overviewService;

        public ClientWriteService(IClientRepository repository, ICpfService cpfService, IClientOverviewService overviewService)
        {
            _repository = repository;
            _cpfService = cpfService;
            _overviewService = overviewService;
        }

        public async Task<ClientOverviewDto?> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default)
        {
            var normalized = _cpfService.Normalize(dto.Cpf);
            if (!_cpfService.IsValid(normalized))
                throw new ArgumentException("CPF inválido", nameof(dto.Cpf));

            var exists = await _repository.GetByCpfAsync(normalized, cancellationToken);
            if (exists is not null)
                throw new InvalidOperationException("CPF já cadastrado");

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Cpf = normalized,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(client, cancellationToken);

            return await _overviewService.GetOverviewAsync(client.Id, cancellationToken);
        }
    }
}
