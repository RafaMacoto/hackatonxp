using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;

namespace Advisor360.Application.Interfaces
{
    public interface IExternalProfileProvider
    {
        /// <summary>
        /// Tenta recuperar dados externos para o cliente informado.
        /// Implementações devem ser resilientes e não lançar em falhas transitórias; use o resultado com Success=false.
        /// </summary>
        Task<ExternalProfileResult> GetProfileAsync(Guid clientId, CancellationToken cancellationToken = default);
    }
}
