namespace WintunerDashboard.WebApi.Models;

public record PackageItem(
    string Id,
    string Name,
    string Publisher,
    string CurrentVersion,
    string Status,
    DateTimeOffset LastUpdated,
    IReadOnlyList<PackageVersionItem> Versions);

public record PackageVersionItem(
    string Version,
    string ReleaseChannel,
    DateTimeOffset PublishedAt,
    string Sha256);
