using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IAdvisorService
    {
        Task<AdvisorDto> CreateAsync(AdvisorCreateDto dto, CancellationToken cancellationToken = default);
        Task<AdvisorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
