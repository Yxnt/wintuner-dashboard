using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Infrastructure.Persistence;
using WintunerDashboard.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WintunerDbContext>(options =>
{
    var configuration = builder.Configuration;
    var host = configuration["POSTGRES_HOST"] ?? "localhost";
    var port = configuration["POSTGRES_PORT"] ?? "5432";
    var database = configuration["POSTGRES_DB"] ?? "wintuner";
    var user = configuration["POSTGRES_USER"] ?? "wintuner";
    var password = configuration["POSTGRES_PASSWORD"] ?? "change_me";
    var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok("ok"));
app.MapApiEndpoints();

app.MapGet("/api/dashboard", (InMemoryDataStore store) =>
{
    var summary = store.GetDashboardSummary();
    return Results.Ok(summary);
});

app.MapGet("/api/packages", (InMemoryDataStore store) => Results.Ok(store.GetPackages()));

app.MapGet("/api/packages/{packageId}", (string packageId, InMemoryDataStore store) =>
{
    var package = store.GetPackage(packageId);
    return package is null ? Results.NotFound() : Results.Ok(package);
});

app.MapGet("/api/publish-requests", (InMemoryDataStore store) => Results.Ok(store.GetPublishRequests()));

app.MapPost("/api/publish-requests", (CreatePublishRequestRequest request, InMemoryDataStore store) =>
{
    var created = store.CreatePublishRequest(request);
    return Results.Created($"/api/publish-requests/{created.Id}", created);
});

app.MapGet("/api/jobs", (InMemoryDataStore store) => Results.Ok(store.GetJobs()));

app.MapGet("/api/settings", (InMemoryDataStore store) => Results.Ok(store.GetSettings()));

app.Run();

record DashboardSummary(
    DashboardStats Stats,
    IReadOnlyList<PublishRequestItem> RecentPublishRequests,
    IReadOnlyList<JobItem> RecentJobs);

record DashboardStats(
    int TotalPackages,
    int PendingPublishRequests,
    int RunningJobs,
    int FailedJobs,
    int PublishedApps);

record PackageItem(
    string Id,
    string Name,
    string Publisher,
    string CurrentVersion,
    string Status,
    DateTimeOffset LastUpdated,
    IReadOnlyList<PackageVersionItem> Versions);

record PackageVersionItem(
    string Version,
    string ReleaseChannel,
    DateTimeOffset PublishedAt,
    string Sha256);

record PublishRequestItem(
    string Id,
    string PackageId,
    string PackageName,
    string Version,
    string Target,
    string Intent,
    string Status,
    DateTimeOffset RequestedAt,
    string RequestedBy);

record JobItem(
    string Id,
    string PackageId,
    string PackageName,
    string Version,
    string Step,
    string Status,
    int Progress,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt);

record SettingsResponse(
    string TenantName,
    string NotificationMailbox,
    string DefaultTarget,
    string Environment,
    IReadOnlyList<string> AvailableGroups);

record CreatePublishRequestRequest(
    string PackageId,
    string Version,
    string Target,
    string Intent,
    string RequestedBy);

sealed class InMemoryDataStore
{
    private readonly ConcurrentDictionary<string, PackageItem> _packages = new();
    private readonly ConcurrentQueue<PublishRequestItem> _publishRequests = new();
    private readonly ConcurrentQueue<JobItem> _jobs = new();

    public InMemoryDataStore()
    {
        var packages = new[]
        {
            new PackageItem(
                "microsoft-teams",
                "Microsoft Teams",
                "Microsoft",
                "24102.2509.3205.8967",
                "Published",
                DateTimeOffset.UtcNow.AddDays(-1),
                new List<PackageVersionItem>
                {
                    new("24102.2509.3205.8967", "Stable", DateTimeOffset.UtcNow.AddDays(-1), "8b62...9f3a"),
                    new("24095.2309.3102.5470", "Stable", DateTimeOffset.UtcNow.AddDays(-8), "84aa...1c2e")
                }),
            new PackageItem(
                "vscode",
                "Visual Studio Code",
                "Microsoft",
                "1.87.2",
                "Update Available",
                DateTimeOffset.UtcNow.AddDays(-2),
                new List<PackageVersionItem>
                {
                    new("1.87.2", "Stable", DateTimeOffset.UtcNow.AddDays(-2), "f1d2...b4c9"),
                    new("1.86.0", "Stable", DateTimeOffset.UtcNow.AddDays(-20), "17af...2bcd")
                }),
            new PackageItem(
                "7zip",
                "7-Zip",
                "Igor Pavlov",
                "23.01",
                "Pending Publish",
                DateTimeOffset.UtcNow.AddDays(-3),
                new List<PackageVersionItem>
                {
                    new("23.01", "Stable", DateTimeOffset.UtcNow.AddDays(-3), "9f77...e102")
                })
        };

        foreach (var package in packages)
        {
            _packages.TryAdd(package.Id, package);
        }

        var requests = new[]
        {
            new PublishRequestItem(
                "pr-101",
                "7zip",
                "7-Zip",
                "23.01",
                "Finance Devices",
                "Available",
                "Pending",
                DateTimeOffset.UtcNow.AddHours(-5),
                "alex@contoso.com"),
            new PublishRequestItem(
                "pr-102",
                "vscode",
                "Visual Studio Code",
                "1.87.2",
                "All Staff",
                "Required",
                "Approved",
                DateTimeOffset.UtcNow.AddHours(-12),
                "maria@contoso.com")
        };

        foreach (var request in requests)
        {
            _publishRequests.Enqueue(request);
        }

        var jobs = new[]
        {
            new JobItem(
                "job-410",
                "vscode",
                "Visual Studio Code",
                "1.87.2",
                "Assigning",
                "Running",
                65,
                DateTimeOffset.UtcNow.AddMinutes(-40),
                null),
            new JobItem(
                "job-409",
                "microsoft-teams",
                "Microsoft Teams",
                "24102.2509.3205.8967",
                "Completed",
                "Succeeded",
                100,
                DateTimeOffset.UtcNow.AddHours(-6),
                DateTimeOffset.UtcNow.AddHours(-5).AddMinutes(-40))
        };

        foreach (var job in jobs)
        {
            _jobs.Enqueue(job);
        }
    }

    public DashboardSummary GetDashboardSummary()
    {
        var publishRequests = _publishRequests.ToArray();
        var jobs = _jobs.ToArray();
        var stats = new DashboardStats(
            _packages.Count,
            publishRequests.Count(request => request.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)),
            jobs.Count(job => job.Status.Equals("Running", StringComparison.OrdinalIgnoreCase)),
            jobs.Count(job => job.Status.Equals("Failed", StringComparison.OrdinalIgnoreCase)),
            publishedApps: _packages.Count(package => package.Status.Equals("Published", StringComparison.OrdinalIgnoreCase)));

        return new DashboardSummary(
            stats,
            publishRequests.OrderByDescending(request => request.RequestedAt).Take(5).ToList(),
            jobs.OrderByDescending(job => job.StartedAt).Take(5).ToList());
    }

    public IReadOnlyList<PackageItem> GetPackages() =>
        _packages.Values.OrderBy(package => package.Name).ToList();

    public PackageItem? GetPackage(string packageId) =>
        _packages.TryGetValue(packageId, out var package) ? package : null;

    public IReadOnlyList<PublishRequestItem> GetPublishRequests() =>
        _publishRequests.OrderByDescending(request => request.RequestedAt).ToList();

    public PublishRequestItem CreatePublishRequest(CreatePublishRequestRequest request)
    {
        var package = GetPackage(request.PackageId);
        var item = new PublishRequestItem(
            $"pr-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}",
            request.PackageId,
            package?.Name ?? request.PackageId,
            request.Version,
            request.Target,
            request.Intent,
            "Pending",
            DateTimeOffset.UtcNow,
            request.RequestedBy);

        _publishRequests.Enqueue(item);
        return item;
    }

    public IReadOnlyList<JobItem> GetJobs() =>
        _jobs.OrderByDescending(job => job.StartedAt).ToList();

    public SettingsResponse GetSettings() =>
        new(
            "Contoso - West",
            "wintuner-notify@contoso.com",
            "All Staff",
            "Development",
            new List<string> { "All Staff", "Finance Devices", "Engineering Devices", "Field Workers" });
}
