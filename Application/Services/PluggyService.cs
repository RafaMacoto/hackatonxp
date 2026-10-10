using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace hackatonxp.Infrastructure.Services
{
    public class PluggyService : IPluggyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly SemaphoreSlim _authLock = new(1, 1);

        private string? _apiKey;
        private DateTimeOffset _apiKeyExpiresAt = DateTimeOffset.MinValue;

        public PluggyService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _clientId = configuration["PLUGGY_CLIENT_ID"]
                ?? throw new InvalidOperationException(
                    "PLUGGY_CLIENT_ID não configurado.");

            _clientSecret = configuration["PLUGGY_CLIENT_SECRET"]
                ?? throw new InvalidOperationException(
                    "PLUGGY_CLIENT_SECRET não configurado.");
        }

        private async Task<string> GetApiKeyAsync(
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_apiKey) &&
                DateTimeOffset.UtcNow < _apiKeyExpiresAt)
            {
                return _apiKey;
            }

            await _authLock.WaitAsync(cancellationToken);

            try
            {
                if (!string.IsNullOrWhiteSpace(_apiKey) &&
                    DateTimeOffset.UtcNow < _apiKeyExpiresAt)
                {
                    return _apiKey;
                }

                using var response = await _httpClient.PostAsJsonAsync(
                    "auth",
                    new
                    {
                        clientId = _clientId,
                        clientSecret = _clientSecret
                    },
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var authResponse =
                    await response.Content.ReadFromJsonAsync<PluggyAuthResponse>(
                        cancellationToken: cancellationToken);

                if (string.IsNullOrWhiteSpace(authResponse?.ApiKey))
                {
                    throw new InvalidOperationException(
                        "A Pluggy não retornou uma API Key válida.");
                }

                _apiKey = authResponse.ApiKey;

                _apiKeyExpiresAt = DateTimeOffset.UtcNow
                    .AddHours(1)
                    .AddMinutes(55);

                return _apiKey;
            }
            finally
            {
                _authLock.Release();
            }
        }

        public async Task<List<InvestmentResponse>> GetClientInvestmentsAsync(
            string itemId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException(
                    "O itemId é obrigatório.",
                    nameof(itemId));
            }

            var apiKey = await GetApiKeyAsync(cancellationToken);

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"investments?itemId={Uri.EscapeDataString(itemId)}");

            request.Headers.Add("X-API-KEY", apiKey);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<PluggyInvestmentsResponse>(
                    cancellationToken: cancellationToken);

            return result?.Results ?? new List<InvestmentResponse>();
        }

        private sealed class PluggyAuthResponse
        {
            [JsonPropertyName("apiKey")]
            public string? ApiKey { get; set; }
        }

        private sealed class PluggyInvestmentsResponse
        {
            [JsonPropertyName("results")]
            public List<InvestmentResponse> Results { get; set; } = new();
        }
    }
}