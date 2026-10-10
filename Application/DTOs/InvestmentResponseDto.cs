using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace hackatonxp.Application.DTOs
{
    public class InvestmentResponseDto
    {
        public string? Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public decimal? Balance { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Type { get; set; }
        public string? Subtype { get; set; }

        public decimal? LastMonthRate { get; set; }
        public decimal? LastTwelveMonthsRate { get; set; }
        public decimal? AnnualRate { get; set; }

        public string? ItemId { get; set; }
        public string? Code { get; set; }
        public string? Isin { get; set; }

        public JsonElement? Metadata { get; set; }
        public decimal? Value { get; set; }
        public decimal? Quantity { get; set; }

        public decimal? Amount { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Taxes2 { get; set; }
        public decimal? AmountProfit { get; set; }
        public decimal? AmountWithdrawal { get; set; }
        public decimal? AmountOriginal { get; set; }

        public DateTimeOffset? Date { get; set; }
        public string? Owner { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public string? Issuer { get; set; }
        public string? IssuerCNPJ { get; set; }

        public DateTimeOffset? IssueDate { get; set; }
        public DateTimeOffset? PurchaseDate { get; set; }
        public DateTimeOffset? GracePeriodDate { get; set; }

        public decimal? Rate { get; set; }
        public string? RateType { get; set; }
        public string? RatePeriodicity { get; set; }
        public string? IndexerAdditionalInfo { get; set; }
        public decimal? FixedAnnualRate { get; set; }
        public bool? TaxExempt { get; set; }
        public decimal? PriceFactor { get; set; }

        public JsonElement? Debtor { get; set; }
        public JsonElement? CouponPayment { get; set; }
        public string? Status { get; set; }
        public JsonElement? Institution { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}