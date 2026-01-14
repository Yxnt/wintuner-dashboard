using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class IndexSyncRun
{
    [Key]
    public Guid Id { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? FinishedAt { get; set; }

    public string? ETag { get; set; }

    public string Status { get; set; } = "Success";

    public string DiffSummary { get; set; } = "{}";

    public string? Error { get; set; }
}
