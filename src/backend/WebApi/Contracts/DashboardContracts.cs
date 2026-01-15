namespace WintunerDashboard.WebApi.Contracts;

public record DashboardSummaryResponse(
    int Packages,
    int UpdatesPending,
    int PublishRequestsPending,
    int JobsRunning);

public record PackageSummaryResponse(
    Guid Id,
    string Identifier,
    string DisplayName,
    string Publisher,
    string LatestVersion,
    DateTime UpdatedAt);

public record UpdateEventResponse(
    Guid Id,
    string PackageIdentifier,
    string PackageDisplayName,
    string Version,
    string Status,
    DateTime CreatedAt);

public record PublishRequestResponse(
    Guid Id,
    string PackageDisplayName,
    string Version,
    string Intent,
    string Status,
    string RequestedBy,
    DateTime CreatedAt);

public record PublishJobResponse(
    Guid Id,
    Guid PublishRequestId,
    string Status,
    string Step,
    int Attempt,
    DateTime CreatedAt);

public record TenantSettingsResponse(
    Guid Id,
    string Name,
    string EntraTenantId,
    string GraphClientId,
    string GraphAuthMode,
    string NotificationMailbox,
    string NotificationRecipients,
    int SyncIntervalMinutes);
