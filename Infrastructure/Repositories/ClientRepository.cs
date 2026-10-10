using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Domain.Entities;
using Advisor360.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Advisor360.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly Advisor360DbContext _db;

        public ClientRepository(Advisor360DbContext db)
        {
            _db = db;
        }

        public async Task<Client?> GetByIdWithRelatedAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Clients
                .AsNoTracking()
                .Include(c => c.Positions)
                .Include(c => c.Interactions)
                .Include(c => c.Tasks)
                .Include(c => c.ImportantDates)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Client?> GetByCpfAsync(string normalizedCpf, CancellationToken cancellationToken = default)
        {
            return await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Cpf == normalizedCpf, cancellationToken);
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            await _db.Clients.AddAsync(client, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
