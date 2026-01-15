namespace WintunerDashboard.WebApi.Models;

public record JobItem(
    string Id,
    string PackageId,
    string PackageName,
    string Version,
    string Step,
    string Status,
    int Progress,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt);
