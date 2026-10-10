using System;

namespace Advisor360.Domain.Entities
{
    public enum InteractionChannel
    {
        Email,
        Phone,
        Meeting,
        Chat,
        Other
    }

    public class Interaction
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public DateTime Date { get; set; }
        public InteractionChannel Channel { get; set; }
        public string Summary { get; set; } = null!;
        public string? Topics { get; set; }
        public string? NextSteps { get; set; }
    }
}
