using System.ComponentModel.DataAnnotations;

namespace WintunerDashboard.Domain.Entities;

public class AuditLog
{
    [Key]
    public Guid Id { get; set; }

    public string ActorOid { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string EntityType { get; set; } = string.Empty;

    public string EntityId { get; set; } = string.Empty;

    public string Payload { get; set; } = "{}";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
