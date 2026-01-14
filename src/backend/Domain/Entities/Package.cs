using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class Package
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string PackageIdentifier { get; set; } = string.Empty;

    public string? Moniker { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string Publisher { get; set; } = string.Empty;

    public string Tags { get; set; } = "[]";

    public string Source { get; set; } = "winget";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PackageVersion> Versions { get; set; } = new List<PackageVersion>();
}
