
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<ClientDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ClientDto> CreateAsync(
            ClientCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<ClientDto?> UpdateAsync(
            Guid id,
            ClientUpdateDto dto,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
