namespace WintunerDashboard.WebApi.Models;

public record UpdateEventItem(
    string Id,
    string PackageId,
    string PackageName,
    string CurrentVersion,
    string LatestVersion,
    string Status,
    string Severity,
    DateTimeOffset DetectedAt,
    string ActionOwner);
