using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Domain.Entities;
using hackatonxp.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
namespace hackatonxp.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly ICpfService _cpfService;

        public ClientService(
            IClientRepository repository,
            ICpfService cpfService)
        {
            _repository = repository;
            _cpfService = cpfService;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var clients = await _repository.GetAllAsync(cancellationToken);

            return clients.Select(MapToDto).ToList();
        }

        public async Task<ClientDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var client = await _repository.GetByIdAsync(id, cancellationToken);

            return client is null ? null : MapToDto(client);
        }

        public async Task<ClientDto> CreateAsync(
            ClientCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            var cpf = _cpfService.Normalize(dto.Cpf);

            if (!_cpfService.IsValid(cpf))
                throw new ArgumentException("CPF inválido.");

            var existing = await _repository.GetByCpfAsync(
                cpf, cancellationToken);

            if (existing is not null)
                throw new InvalidOperationException(
                    "Já existe um cliente cadastrado com esse CPF.");

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Cpf = cpf,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(client, cancellationToken);

            return MapToDto(client);
        }

        public async Task<ClientDto?> UpdateAsync(
            Guid id,
            ClientUpdateDto dto,
            CancellationToken cancellationToken = default)
        {
            var client = await _repository.GetByIdAsync(
                id, cancellationToken);

            if (client is null)
                return null;

            var cpf = _cpfService.Normalize(dto.Cpf);

            if (!_cpfService.IsValid(cpf))
                throw new ArgumentException("CPF inválido.");

            var existing = await _repository.GetByCpfAsync(
                cpf, cancellationToken);

            if (existing is not null && existing.Id != id)
                throw new InvalidOperationException(
                    "Já existe outro cliente cadastrado com esse CPF.");

            client.Name = dto.Name.Trim();
            client.Cpf = cpf;
            client.Email = dto.Email;
            client.Phone = dto.Phone;
            client.BirthDate = dto.BirthDate;

            await _repository.UpdateAsync(client, cancellationToken);

            return MapToDto(client);
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var client = await _repository.GetByIdAsync(
                id, cancellationToken);

            if (client is null)
                return false;

            await _repository.DeleteAsync(client, cancellationToken);

            return true;
        }

        private static ClientDto MapToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                BirthDate = client.BirthDate,
                InvestorProfile = client.InvestorProfile?.ToString()
            };
        }
    }
}