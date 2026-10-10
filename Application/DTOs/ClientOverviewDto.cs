using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hackatonxp.Application.DTOs
{
    public class ClientOverviewDto
    {
        public ClientDto Client { get; set; } = null!;
        public decimal PortfolioTotal { get; set; }
        public IEnumerable<PositionDto> Positions { get; set; } = Array.Empty<PositionDto>();
        public IEnumerable<InteractionDto> RecentInteractions { get; set; } = Array.Empty<InteractionDto>();
        public IEnumerable<TaskDto> OpenTasks { get; set; } = Array.Empty<TaskDto>();
        public IEnumerable<ImportantDateDto> UpcomingDates { get; set; } = Array.Empty<ImportantDateDto>();
    }

    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? InvestorProfile { get; set; }
    }

    public class ClientCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Cpf { get; set; } = null!;
    }

    public class PositionDto
    {
        public string AssetName { get; set; } = null!;
        public string AssetType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal MarketValue { get; set; }
    }

    public class InteractionDto
    {
        public DateTime Date { get; set; }
        public string Channel { get; set; } = null!;
        public string Summary { get; set; } = null!;
    }

    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = null!;
    }

    public class ImportantDateDto
    {
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Category { get; set; } = null!;
        public string? Notes { get; set; }
    }

    public class ClientUpdateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Cpf { get; set; } = string.Empty;
    }
}
