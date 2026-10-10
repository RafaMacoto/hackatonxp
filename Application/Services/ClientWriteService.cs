using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Domain.Entities;
using hackatonxp.Infrastructure.Repositories;

namespace hackatonxp.Application.Services
{
    public class ClientWriteService
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

        public async Task<ClientDto> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default)
        {
            var cpf = _cpfService.Normalize(dto.Cpf);
            if (!_cpfService.IsValid(cpf))
                throw new ArgumentException("CPF inválido");

            var existing = await _repository.GetByCpfAsync(cpf, cancellationToken);
            if (existing is not null)
                throw new InvalidOperationException("CPF já cadastrado");

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Cpf = cpf,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(client, cancellationToken);

            return new ClientDto { Id = client.Id, Name = client.Name, Email = client.Email, Phone = client.Phone, BirthDate = client.BirthDate };
        }
    }
}
