using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Domain.Entities;
using hackatonxp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace hackatonxp.Infrastructure.Repositories
{
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly HackatonXpDbContext _db;

        public AdvisorRepository(HackatonXpDbContext db)
        {
            _db = db;
        }

        public async Task<Advisor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Advisors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Advisor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Advisors.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
        }

        public async Task AddAsync(Advisor advisor, CancellationToken cancellationToken = default)
        {
            await _db.Advisors.AddAsync(advisor, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
