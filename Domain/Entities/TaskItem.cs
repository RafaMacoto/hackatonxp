using System;

namespace hackatonxp.Domain.Entities
{
    public enum TaskItemStatus
    {
        Open,
        InProgress,
        Completed,
        Cancelled
    }

    public class TaskItem
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskItemStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
