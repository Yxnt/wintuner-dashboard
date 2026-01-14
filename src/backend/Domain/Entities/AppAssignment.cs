using System.ComponentModel.DataAnnotations;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.Entities;

public class AppAssignment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public string IntuneAppId { get; set; } = string.Empty;

    public TargetType TargetType { get; set; }

    [Required]
    public string TargetId { get; set; } = string.Empty;

    public InstallIntent Intent { get; set; }

    public string? GraphAssignmentId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
