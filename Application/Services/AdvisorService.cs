using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Domain.Entities;
using hackatonxp.Infrastructure.Repositories;

namespace hackatonxp.Application.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IAdvisorRepository _repo;
        private readonly IPasswordHasher _hasher;

        public AdvisorService(IAdvisorRepository repo, IPasswordHasher hasher)
        {
            _repo = repo;
            _hasher = hasher;
        }

        public async Task<AdvisorDto> CreateAsync(AdvisorCreateDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email, cancellationToken);
            if (existing is not null)
                throw new InvalidOperationException("Email already registered");

            var advisor = new Advisor
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _hasher.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _repo.AddAsync(advisor, cancellationToken);

            return new AdvisorDto { Id = advisor.Id, Name = advisor.Name, Email = advisor.Email, CreatedAt = advisor.CreatedAt, IsActive = advisor.IsActive };
        }

        public async Task<AdvisorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var a = await _repo.GetByIdAsync(id, cancellationToken);
            if (a is null) return null;
            return new AdvisorDto { Id = a.Id, Name = a.Name, Email = a.Email, CreatedAt = a.CreatedAt, IsActive = a.IsActive };
        }
    }
}
