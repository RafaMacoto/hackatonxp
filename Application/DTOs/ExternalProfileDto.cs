using System;

namespace hackatonxp.Application.DTOs
{
    public class ExternalProfileDto
    {
        public string Source { get; set; } = null!;
        public int? RiskScore { get; set; }
        public int? ExternalAccounts { get; set; }
        public DateTime RetrievedAt { get; set; }
    }

    public class ExternalProfileResult
    {
        public bool Success { get; set; }
        public ExternalProfileDto? Data { get; set; }
        public string? Error { get; set; }
    }
}
