using System.ComponentModel.DataAnnotations;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.Entities;

public class PublishJob
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid PublishRequestId { get; set; }

    public string? HangfireJobId { get; set; }

    public PublishJobStatus Status { get; set; } = PublishJobStatus.Queued;

    public PublishJobStep Step { get; set; } = PublishJobStep.Resolve;

    public int Attempt { get; set; } = 1;

    public string Logs { get; set; } = string.Empty;

    public string? Error { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public PublishRequest? PublishRequest { get; set; }
}
