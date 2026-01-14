using System.ComponentModel.DataAnnotations;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.Entities;

public class PublishRequest
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public Guid PackageId { get; set; }

    [Required]
    public Guid PackageVersionId { get; set; }

    public InstallIntent Intent { get; set; } = InstallIntent.Available;

    public string Targets { get; set; } = "[]";

    public string TargetsNormalized { get; set; } = "[]";

    public PublishRequestStatus Status { get; set; } = PublishRequestStatus.Draft;

    public string RequestedBy { get; set; } = string.Empty;

    public string? ApprovedBy { get; set; }

    public string? ApprovalNote { get; set; }

    [Required]
    public string IdempotencyKey { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Tenant? Tenant { get; set; }

    public Package? Package { get; set; }

    public PackageVersion? PackageVersion { get; set; }
}
