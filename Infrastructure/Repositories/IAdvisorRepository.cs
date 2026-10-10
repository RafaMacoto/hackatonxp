using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Domain.Entities;

namespace hackatonxp.Infrastructure.Repositories
{
    public interface IAdvisorRepository
    {
        Task<Advisor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Advisor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(Advisor advisor, CancellationToken cancellationToken = default);
    }
}
