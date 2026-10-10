
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace hackatonxp.Application.DTOs
{
    public class LoanResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("itemId")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("contractNumber")]
        public string? ContractNumber { get; set; }

        [JsonPropertyName("ipocCode")]
        public string? IpocCode { get; set; }

        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("productSubTypeCategory")]
        public string? ProductSubTypeCategory { get; set; }

        [JsonPropertyName("hasInsuranceContracted")]
        public bool? HasInsuranceContracted { get; set; }

        [JsonPropertyName("totalRemainingAmount")]
        public decimal? TotalRemainingAmount { get; set; }

        [JsonPropertyName("nextInstallmentAmount")]
        public decimal? NextInstallmentAmount { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset? Date { get; set; }

        [JsonPropertyName("contractDate")]
        public DateTimeOffset? ContractDate { get; set; }

        [JsonPropertyName("disbursementDates")]
        public List<DateTimeOffset> DisbursementDates { get; set; } = new();

        [JsonPropertyName("settlementDate")]
        public DateTimeOffset? SettlementDate { get; set; }

        [JsonPropertyName("contractAmount")]
        public decimal? ContractAmount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("dueDate")]
        public DateTimeOffset? DueDate { get; set; }

        [JsonPropertyName("installmentPeriodicity")]
        public string? InstallmentPeriodicity { get; set; }

        [JsonPropertyName("CET")]
        public decimal? CET { get; set; }

        [JsonPropertyName("amortizationScheduled")]
        public string? AmortizationScheduled { get; set; }

        [JsonPropertyName("amortizationScheduledAdditionalInfo")]
        public string? AmortizationScheduledAdditionalInfo { get; set; }

        [JsonPropertyName("cnpjConsignee")]
        public string? CnpjConsignee { get; set; }

        [JsonPropertyName("interestRates")]
        public List<LoanInterestRateResponse> InterestRates { get; set; } = new();
    }

    public class LoanInterestRateResponse
    {
        [JsonPropertyName("additionalInfo")]
        public string? AdditionalInfo { get; set; }

        [JsonPropertyName("calculation")]
        public string? Calculation { get; set; }

        [JsonPropertyName("interestRateType")]
        public string? InterestRateType { get; set; }

        [JsonPropertyName("postFixedRate")]
        public decimal? PostFixedRate { get; set; }

        [JsonPropertyName("preFixedRate")]
        public decimal? PreFixedRate { get; set; }

        [JsonPropertyName("referentialRateIndexerAdditionalInfo")]
        public string? ReferentialRateIndexerAdditionalInfo { get; set; }

        [JsonPropertyName("referentialRateIndexerSubType")]
        public string? ReferentialRateIndexerSubType { get; set; }

        [JsonPropertyName("referentialRateIndexerType")]
        public string? ReferentialRateIndexerType { get; set; }

        [JsonPropertyName("taxPeriodicity")]
        public string? TaxPeriodicity { get; set; }

        [JsonPropertyName("taxType")]
        public string? TaxType { get; set; }
    }
}
