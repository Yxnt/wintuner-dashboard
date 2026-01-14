using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class Tenant
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string EntraTenantId { get; set; } = string.Empty;

    [Required]
    public string GraphClientId { get; set; } = string.Empty;

    [Required]
    public string GraphAuthMode { get; set; } = "client_secret";

    public string? GraphSecretEncrypted { get; set; }

    public string? CertificateThumbprint { get; set; }

    public string NotificationMailbox { get; set; } = string.Empty;

    public string NotificationRecipients { get; set; } = "[]";

    public int SyncIntervalMinutes { get; set; } = 300;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
