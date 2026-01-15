namespace WintunerDashboard.WebApi.Models;

public record PublishRequestItem(
    string Id,
    string PackageId,
    string PackageName,
    string Version,
    string Target,
    string Intent,
    string Status,
    DateTimeOffset RequestedAt,
    string RequestedBy);

public record CreatePublishRequestRequest(
    string PackageId,
    string Version,
    string Target,
    string Intent,
    string RequestedBy);
