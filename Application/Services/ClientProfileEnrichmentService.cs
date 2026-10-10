using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;

namespace hackatonxp.Application.Services
{
    public class ClientProfileEnrichmentService
    {
        private readonly IEnumerable<IExternalProfileProvider> _providers;

        public ClientProfileEnrichmentService(IEnumerable<IExternalProfileProvider> providers)
        {
            _providers = providers;
        }

        public async Task<IEnumerable<ExternalProfileResult>> EnrichAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            var tasks = _providers.Select(p => p.GetProfileAsync(clientId, cancellationToken));
            var results = await Task.WhenAll(tasks);
            return results;
        }
    }
}
