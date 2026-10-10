
using System.Net.Http.Json;
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

            _clientId =
                configuration["Pluggy:ClientId"]
                ?? configuration["PLUGGY_CLIENT_ID"]
                ?? Environment.GetEnvironmentVariable("PLUGGY_CLIENT_ID")
                ?? throw new InvalidOperationException(
                    "Credencial da Pluggy não configurada. " +
                    "Configure Pluggy:ClientId nos User Secrets.");

            _clientSecret =
                configuration["Pluggy:ClientSecret"]
                ?? configuration["PLUGGY_CLIENT_SECRET"]
                ?? Environment.GetEnvironmentVariable("PLUGGY_CLIENT_SECRET")
                ?? throw new InvalidOperationException(
                    "Credencial da Pluggy não configurada. " +
                    "Configure Pluggy:ClientSecret nos User Secrets.");
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

                // Renova a chave antes da expiração estimada.
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

        public async Task<List<InvestmentResponseDto>> GetClientInvestmentsAsync(
            string itemId,
            CancellationToken cancellationToken = default)
        {
            ValidateItemId(itemId);

            var apiKey = await GetApiKeyAsync(cancellationToken);

            using var request = CreateRequest(
                $"investments?itemId={Uri.EscapeDataString(itemId)}",
                apiKey);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<PluggyInvestmentsResponse>(
                    cancellationToken: cancellationToken);

            return result?.Results ?? new List<InvestmentResponseDto>();
        }

        public async Task<List<LoanResponse>> GetClientLoansAsync(
            string itemId,
            CancellationToken cancellationToken = default)
        {
            ValidateItemId(itemId);

            var apiKey = await GetApiKeyAsync(cancellationToken);

            using var request = CreateRequest(
                $"loans?itemId={Uri.EscapeDataString(itemId)}",
                apiKey);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<PluggyLoansResponse>(
                    cancellationToken: cancellationToken);

            return result?.Results ?? new List<LoanResponse>();
        }

        // Consulta os dados do item/conexão bancária.
        // Este endpoint não representa necessariamente uma lista de contas.
        public async Task<AccountResponseDto?> GetClientAccountAsync(
            string itemId,
            CancellationToken cancellationToken = default)
        {
            ValidateItemId(itemId);

            var apiKey = await GetApiKeyAsync(cancellationToken);

            using var request = CreateRequest(
                $"items/{Uri.EscapeDataString(itemId)}",
                apiKey);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AccountResponseDto>(
                cancellationToken: cancellationToken);
        }

        private static HttpRequestMessage CreateRequest(
            string endpoint,
            string apiKey)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                endpoint);

            request.Headers.Add("X-API-KEY", apiKey);

            return request;
        }

        private static void ValidateItemId(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException(
                    "O itemId é obrigatório.",
                    nameof(itemId));
            }
        }

        private sealed class PluggyAuthResponse
        {
            [JsonPropertyName("apiKey")]
            public string? ApiKey { get; set; }
        }

        private sealed class PluggyInvestmentsResponse
        {
            [JsonPropertyName("results")]
            public List<InvestmentResponseDto> Results { get; set; } = new();
        }

        private sealed class PluggyLoansResponse
        {
            [JsonPropertyName("results")]
            public List<LoanResponse> Results { get; set; } = new();
        }
    }
}
