using System;

namespace hackatonxp.Domain.Entities
{
    public enum AssetType
    {
        Stock,
        Bond,
        Fund,
        Cash,
        Other
    }

    public class PortfolioPosition
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public string AssetName { get; set; } = null!;
        public AssetType AssetType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
