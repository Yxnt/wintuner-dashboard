using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class IntuneApp
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public Guid PackageId { get; set; }

    [Required]
    public string IntuneAppId { get; set; } = string.Empty;

    public string AppName { get; set; } = string.Empty;

    public string CurrentVersion { get; set; } = string.Empty;

    public DateTime? LastPublishedAt { get; set; }

    public Tenant? Tenant { get; set; }

    public Package? Package { get; set; }
}
