using System.ComponentModel.DataAnnotations;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.Entities;

public class UpdateEvent
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid PackageId { get; set; }

    [Required]
    public Guid PackageVersionId { get; set; }

    [Required]
    public string EventType { get; set; } = string.Empty;

    public UpdateEventStatus Status { get; set; } = UpdateEventStatus.PendingApproval;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? HandledAt { get; set; }

    public string? HandledBy { get; set; }

    public string? Note { get; set; }

    public Package? Package { get; set; }

    public PackageVersion? PackageVersion { get; set; }
}
