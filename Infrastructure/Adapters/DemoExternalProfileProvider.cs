using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.DTOs;
using Advisor360.Application.Interfaces;

namespace Advisor360.Infrastructure.Adapters
{
    // Implementação de demonstração — não registrada automaticamente. Serve como exemplo de adaptador HTTP tipado.
    public class DemoExternalProfileProvider : IExternalProfileProvider
    {
        private readonly HttpClient _http;

        public DemoExternalProfileProvider(HttpClient http)
        {
            _http = http;
        }

        public Task<ExternalProfileResult> GetProfileAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            // Não chamar qualquer API real aqui. Retorna resultado fictício demonstrativo.
            var dto = new ExternalProfileDto
            {
                Source = "demo",
                RiskScore = 5,
                ExternalAccounts = 2,
                RetrievedAt = DateTime.UtcNow
            };

            return Task.FromResult(new ExternalProfileResult { Success = true, Data = dto });
        }
    }
}
