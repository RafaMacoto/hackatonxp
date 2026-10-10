
using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IClientOverviewService
    {
        Task<ClientOverviewDto?> GetOverviewAsync(
            Guid clientId,
            CancellationToken cancellationToken = default);
    }
}
