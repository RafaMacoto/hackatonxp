using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;

namespace Advisor360.Application.Interfaces
{
    public interface IClientOverviewService
    {
        Task<ClientOverviewDto?> GetOverviewAsync(Guid clientId, CancellationToken cancellationToken = default);
    }
}
