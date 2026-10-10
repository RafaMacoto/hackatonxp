using System;
using System.Collections.Generic;
using System.Text.Json;

namespace hackatonxp.Application.DTOs
{
    public class AccountResponseDto
    {
        public string? Id { get; set; }

        public JsonElement? Connector { get; set; }

        public int? ConnectorId { get; set; }

        public string? Name { get; set; }
        public string? PrimaryColor { get; set; }
        public string? InstitutionUrl { get; set; }
        public string? Country { get; set; }
        public string? Type { get; set; }

        public List<string>? Credentials { get; set; }

        public string? ImageUrl { get; set; }
        public bool? HasMFA { get; set; }

        public JsonElement? Health { get; set; }

        public List<string>? Products { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool? IsSandbox { get; set; }
        public bool? IsOpenFinance { get; set; }

        public bool? SupportsPaymentInitiation { get; set; }
        public bool? SupportsScheduledPayments { get; set; }
        public bool? SupportsSmartTransfers { get; set; }
        public bool? SupportsBoletoManagement { get; set; }
        public bool? SupportsAutomaticPix { get; set; }

        public string? Status { get; set; }
        public string? ExecutionStatus { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string? WebhookUrl { get; set; }

        public JsonElement? Error { get; set; }

        public string? ClientUserId { get; set; }

        public int? ConsecutiveFailedLoginAttempts { get; set; }

        public string? StatusDetail { get; set; }
        public string? Parameter { get; set; }
        public string? UserAction { get; set; }

        public DateTime? NextAutoSyncAt { get; set; }
        public DateTime? AutoSyncDisabledAt { get; set; }
        public DateTime? ConsentExpiresAt { get; set; }
        public DateTime? ResourcesCollectedAt { get; set; }

        public bool? HasResourcesPendingAuthorization { get; set; }

        public string? OauthRedirectUri { get; set; }
    }
}