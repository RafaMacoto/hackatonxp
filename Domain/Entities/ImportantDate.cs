using System;

namespace Advisor360.Domain.Entities
{
    public enum ImportantDateCategory
    {
        Birthday,
        AccountAnniversary,
        TaxDeadline,
        Other
    }

    public class ImportantDate
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public ImportantDateCategory Category { get; set; }
        public string? Notes { get; set; }
    }
}
