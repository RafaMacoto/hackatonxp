using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;

namespace Advisor360.Application.Interfaces
{
    public interface IClientWriteService
    {
        Task<ClientOverviewDto?> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default);
    }
}
