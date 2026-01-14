using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class PackageVersion
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid PackageId { get; set; }

    [Required]
    public string Version { get; set; } = string.Empty;

    public string InstallerUrl { get; set; } = string.Empty;

    public string InstallerSha256 { get; set; } = string.Empty;

    public string? ManifestUrl { get; set; }

    public string Raw { get; set; } = "{}";

    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;

    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;

    public Package? Package { get; set; }
}
