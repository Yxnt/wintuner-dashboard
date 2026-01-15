using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Domain.Enums;
using WintunerDashboard.Infrastructure.Persistence;
using WintunerDashboard.WebApi.Contracts;

namespace WintunerDashboard.WebApi;

public static class ApiEndpoints
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/dashboard/summary", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var summary = new DashboardSummaryResponse(
                await db.Packages.CountAsync(ct),
                await db.UpdateEvents.CountAsync(e => e.Status == UpdateEventStatus.PendingApproval, ct),
                await db.PublishRequests.CountAsync(r => r.Status == PublishRequestStatus.PendingApproval, ct),
                await db.PublishJobs.CountAsync(j => j.Status == PublishJobStatus.Running, ct)
            );

            return Results.Ok(summary);
        });

        group.MapGet("/packages", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var packages = await db.Packages
                .AsNoTracking()
                .OrderByDescending(p => p.UpdatedAt)
                .Select(p => new PackageSummaryResponse(
                    p.Id,
                    p.PackageIdentifier,
                    p.DisplayName,
                    p.Publisher,
                    p.Versions.OrderByDescending(v => v.LastSeenAt)
                        .Select(v => v.Version)
                        .FirstOrDefault() ?? "-",
                    p.UpdatedAt
                ))
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(packages);
        });

        group.MapGet("/updates", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var updates = await db.UpdateEvents
                .AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new UpdateEventResponse(
                    e.Id,
                    e.Package != null ? e.Package.PackageIdentifier : "-",
                    e.Package != null ? e.Package.DisplayName : "-",
                    e.PackageVersion != null ? e.PackageVersion.Version : "-",
                    e.Status.ToString(),
                    e.CreatedAt
                ))
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(updates);
        });

        group.MapGet("/publish-requests", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var requests = await db.PublishRequests
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new PublishRequestResponse(
                    r.Id,
                    r.Package != null ? r.Package.DisplayName : "-",
                    r.PackageVersion != null ? r.PackageVersion.Version : "-",
                    r.Intent.ToString(),
                    r.Status.ToString(),
                    r.RequestedBy,
                    r.CreatedAt
                ))
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(requests);
        });

        group.MapGet("/jobs", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var jobs = await db.PublishJobs
                .AsNoTracking()
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new PublishJobResponse(
                    j.Id,
                    j.PublishRequestId,
                    j.Status.ToString(),
                    j.Step.ToString(),
                    j.Attempt,
                    j.CreatedAt
                ))
                .Take(50)
                .ToListAsync(ct);

            return Results.Ok(jobs);
        });

        group.MapGet("/settings/tenant", async (WintunerDbContext db, CancellationToken ct) =>
        {
            var tenant = await db.Tenants
                .AsNoTracking()
                .OrderBy(t => t.CreatedAt)
                .Select(t => new TenantSettingsResponse(
                    t.Id,
                    t.Name,
                    t.EntraTenantId,
                    t.GraphClientId,
                    t.GraphAuthMode,
                    t.NotificationMailbox,
                    t.NotificationRecipients,
                    t.SyncIntervalMinutes
                ))
                .FirstOrDefaultAsync(ct);

            return tenant is null ? Results.NotFound() : Results.Ok(tenant);
        });
    }
}
