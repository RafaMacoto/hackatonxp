using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Domain.Entities;

namespace Advisor360.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdWithRelatedAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Client?> GetByCpfAsync(string normalizedCpf, CancellationToken cancellationToken = default);
        Task AddAsync(Client client, CancellationToken cancellationToken = default);
    }
}
