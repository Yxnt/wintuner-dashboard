namespace WintunerDashboard.WebApi.Models;

public record DashboardSummary(
    DashboardStats Stats,
    IReadOnlyList<PublishRequestItem> RecentPublishRequests,
    IReadOnlyList<JobItem> RecentJobs);

public record DashboardStats(
    int TotalPackages,
    int PendingPublishRequests,
    int RunningJobs,
    int FailedJobs,
    int PublishedApps);
