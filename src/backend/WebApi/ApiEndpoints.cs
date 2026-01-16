using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Application.Services;
using WintunerDashboard.Domain.Entities;
using WintunerDashboard.Domain.Enums;
using WintunerDashboard.Domain.ValueObjects;
using WintunerDashboard.Infrastructure.Persistence;
using WintunerDashboard.WebApi.Models;

namespace WintunerDashboard.WebApi;

public static class ApiEndpoints
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/dashboard", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var stats = new DashboardStats(
                await db.Packages.CountAsync(ct),
                await db.PublishRequests.CountAsync(r => r.Status == PublishRequestStatus.PendingApproval, ct),
                await db.PublishJobs.CountAsync(j => j.Status == PublishJobStatus.Running, ct),
                await db.PublishJobs.CountAsync(j => j.Status == PublishJobStatus.Failed, ct),
                await db.IntuneApps.CountAsync(ct));

            var recentRequests = await db.PublishRequests
                .AsNoTracking()
                .Include(r => r.Package)
                .Include(r => r.PackageVersion)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync(ct);

            var recentJobs = await db.PublishJobs
                .AsNoTracking()
                .Include(j => j.PublishRequest)
                .ThenInclude(r => r.Package)
                .Include(j => j.PublishRequest)
                .ThenInclude(r => r.PackageVersion)
                .OrderByDescending(j => j.CreatedAt)
                .Take(5)
                .ToListAsync(ct);

            var summary = new DashboardSummary(
                stats,
                recentRequests.Select(MapPublishRequestItem).ToList(),
                recentJobs.Select(MapJobItem).ToList());

            return Results.Ok(summary);
        });

        group.MapGet("/packages", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var packages = await db.Packages
                .AsNoTracking()
                .Include(p => p.Versions)
                .OrderByDescending(p => p.UpdatedAt)
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(packages.Select(MapPackageItem));
        });

        group.MapGet("/updates", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var updates = await db.UpdateEvents
                .AsNoTracking()
                .Include(e => e.Package)
                .ThenInclude(p => p.Versions)
                .Include(e => e.PackageVersion)
                .OrderByDescending(e => e.CreatedAt)
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(updates.Select(MapUpdateEventItem));
        });

        group.MapGet("/publish-requests", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var requests = await db.PublishRequests
                .AsNoTracking()
                .Include(r => r.Package)
                .Include(r => r.PackageVersion)
                .OrderByDescending(r => r.CreatedAt)
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(requests.Select(MapPublishRequestItem));
        });

        group.MapPost("/publish-requests", async (CreatePublishRequestRequest request, WintunerDbContext db, CancellationToken ct) =>
        {
            var tenant = await db.Tenants.AsNoTracking().OrderBy(t => t.CreatedAt).FirstOrDefaultAsync(ct);
            if (tenant is null)
            {
                return Results.BadRequest(new { message = "Tenant settings are required before creating publish requests." });
            }

            Package? package = null;
            if (Guid.TryParse(request.PackageId, out var packageId))
            {
                package = await db.Packages.FirstOrDefaultAsync(p => p.Id == packageId, ct);
            }
            else
            {
                package = await db.Packages.FirstOrDefaultAsync(p => p.PackageIdentifier == request.PackageId, ct);
            }

            if (package is null)
            {
                return Results.NotFound(new { message = "Package not found." });
            }

            var packageVersion = await db.PackageVersions
                .FirstOrDefaultAsync(v => v.PackageId == package.Id && v.Version == request.Version, ct);

            if (packageVersion is null)
            {
                return Results.NotFound(new { message = "Package version not found." });
            }

            var targets = new List<Target>
            {
                new(TargetType.Group, request.Target)
            };

            var targetsNormalized = TargetsNormalizer.Normalize(targets);
            var targetsSerialized = JsonSerializer.Serialize(targets, SerializerOptions);
            var intent = ParseIntent(request.Intent);
            var idempotencyKey = IdempotencyKeyBuilder.Build(tenant.Id, package.PackageIdentifier, request.Version, intent, targetsNormalized);

            var existing = await db.PublishRequests
                .AsNoTracking()
                .Include(r => r.Package)
                .Include(r => r.PackageVersion)
                .FirstOrDefaultAsync(r => r.IdempotencyKey == idempotencyKey, ct);

            if (existing is not null)
            {
                return Results.Ok(MapPublishRequestItem(existing));
            }

            var publishRequest = new PublishRequest
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                PackageId = package.Id,
                PackageVersionId = packageVersion.Id,
                Intent = intent,
                Targets = targetsSerialized,
                TargetsNormalized = targetsNormalized,
                Status = PublishRequestStatus.PendingApproval,
                RequestedBy = request.RequestedBy,
                IdempotencyKey = idempotencyKey,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.PublishRequests.Add(publishRequest);
            await db.SaveChangesAsync(ct);

            publishRequest.Package = package;
            publishRequest.PackageVersion = packageVersion;

            return Results.Created($"/api/publish-requests/{publishRequest.Id}", MapPublishRequestItem(publishRequest));
        });

        group.MapGet("/jobs", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var jobs = await db.PublishJobs
                .AsNoTracking()
                .Include(j => j.PublishRequest)
                .ThenInclude(r => r.Package)
                .Include(j => j.PublishRequest)
                .ThenInclude(r => r.PackageVersion)
                .OrderByDescending(j => j.CreatedAt)
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(jobs.Select(MapJobItem));
        });

        group.MapGet("/settings", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var tenant = await db.Tenants.AsNoTracking().OrderBy(t => t.CreatedAt).FirstOrDefaultAsync(ct);
            if (tenant is null)
            {
                return Results.NotFound();
            }

            var groups = ParseStringList(tenant.NotificationRecipients);
            var defaultTarget = groups.FirstOrDefault() ?? "All Staff";
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var settings = new SettingsResponse(
                tenant.Name,
                tenant.NotificationMailbox,
                defaultTarget,
                environmentName,
                groups);

            return Results.Ok(settings);
        });
    }

    private static PublishRequestItem MapPublishRequestItem(PublishRequest request)
    {
        var targetLabel = ExtractTargetsLabel(request.TargetsNormalized, request.Targets);
        return new PublishRequestItem(
            request.Id.ToString(),
            request.PackageId.ToString(),
            request.Package?.DisplayName ?? request.PackageId.ToString(),
            request.PackageVersion?.Version ?? "-",
            targetLabel,
            request.Intent.ToString(),
            request.Status.ToString(),
            request.CreatedAt,
            request.RequestedBy);
    }

    private static JobItem MapJobItem(PublishJob job)
    {
        var publishRequest = job.PublishRequest;
        var startedAt = job.StartedAt ?? job.CreatedAt;
        return new JobItem(
            job.Id.ToString(),
            publishRequest?.PackageId.ToString() ?? "-",
            publishRequest?.Package?.DisplayName ?? "-",
            publishRequest?.PackageVersion?.Version ?? "-",
            job.Step.ToString(),
            job.Status.ToString(),
            CalculateProgress(job.Step, job.Status),
            startedAt,
            job.FinishedAt);
    }

    private static PackageItem MapPackageItem(Package package)
    {
        var versions = package.Versions
            .OrderByDescending(v => v.LastSeenAt)
            .Select(v => new PackageVersionItem(
                v.Version,
                "Stable",
                v.LastSeenAt,
                v.InstallerSha256))
            .ToList();

        return new PackageItem(
            package.Id.ToString(),
            package.DisplayName,
            package.Publisher,
            versions.FirstOrDefault()?.Version ?? "-",
            versions.Count > 0 ? "Synced" : "Pending",
            package.UpdatedAt,
            versions);
    }

    private static UpdateEventItem MapUpdateEventItem(UpdateEvent updateEvent)
    {
        var versions = updateEvent.Package?.Versions
            .OrderByDescending(v => v.LastSeenAt)
            .ToList() ?? new List<PackageVersion>();

        var currentVersion = versions.Skip(1).FirstOrDefault()?.Version ?? "-";
        var latestVersion = updateEvent.PackageVersion?.Version ?? versions.FirstOrDefault()?.Version ?? "-";
        var severity = string.IsNullOrWhiteSpace(updateEvent.EventType) ? "Normal" : updateEvent.EventType;

        return new UpdateEventItem(
            updateEvent.Id.ToString(),
            updateEvent.Package?.PackageIdentifier ?? updateEvent.PackageId.ToString(),
            updateEvent.Package?.DisplayName ?? "-",
            currentVersion,
            latestVersion,
            updateEvent.Status.ToString(),
            severity,
            updateEvent.CreatedAt,
            updateEvent.HandledBy ?? "Unassigned");
    }

    private static int CalculateProgress(PublishJobStep step, PublishJobStatus status)
    {
        if (status is PublishJobStatus.Succeeded or PublishJobStatus.Failed)
        {
            return 100;
        }

        var steps = Enum.GetValues<PublishJobStep>().Length;
        var index = Math.Clamp((int)step + 1, 1, steps);
        return (int)Math.Round(index / (double)steps * 100, MidpointRounding.AwayFromZero);
    }

    private static string ExtractTargetsLabel(string targetsNormalized, string targets)
    {
        var parsedTargets = ParseTargets(targetsNormalized);
        if (parsedTargets.Count == 0)
        {
            parsedTargets = ParseTargets(targets);
        }

        if (parsedTargets.Count == 0)
        {
            return "-";
        }

        return string.Join(", ", parsedTargets.Select(target => target.Id));
    }

    private static List<Target> ParseTargets(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Target>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Target>>(json, SerializerOptions) ?? new List<Target>();
        }
        catch (JsonException)
        {
            return new List<Target>();
        }
    }

    private static IReadOnlyList<string> ParseStringList(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json, SerializerOptions) ?? Array.Empty<string>();
        }
        catch (JsonException)
        {
            return Array.Empty<string>();
        }
    }

    private static InstallIntent ParseIntent(string intent)
    {
        if (Enum.TryParse<InstallIntent>(intent, true, out var parsed))
        {
            return parsed;
        }

        return InstallIntent.Available;
    }
}
