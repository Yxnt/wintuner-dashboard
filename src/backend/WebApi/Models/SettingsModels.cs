namespace WintunerDashboard.WebApi.Models;

public record SettingsResponse(
    string TenantName,
    string NotificationMailbox,
    string DefaultTarget,
    string Environment,
    IReadOnlyList<string> AvailableGroups);
