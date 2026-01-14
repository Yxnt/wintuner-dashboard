using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Domain.Entities;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Infrastructure.Persistence;

public class WintunerDbContext : DbContext
{
    public WintunerDbContext(DbContextOptions<WintunerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageVersion> PackageVersions => Set<PackageVersion>();
    public DbSet<IndexSyncRun> IndexSyncRuns => Set<IndexSyncRun>();
    public DbSet<UpdateEvent> UpdateEvents => Set<UpdateEvent>();
    public DbSet<PublishRequest> PublishRequests => Set<PublishRequest>();
    public DbSet<PublishJob> PublishJobs => Set<PublishJob>();
    public DbSet<IntuneApp> IntuneApps => Set<IntuneApp>();
    public DbSet<AppAssignment> AppAssignments => Set<AppAssignment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("tenants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NotificationRecipients).HasColumnType("jsonb");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("packages");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PackageIdentifier).IsUnique();
            entity.Property(e => e.Tags).HasColumnType("jsonb");
            entity.HasIndex(e => e.DisplayName);
            entity.HasIndex(e => e.Publisher);
            entity.HasIndex(e => e.UpdatedAt);
        });

        modelBuilder.Entity<PackageVersion>(entity =>
        {
            entity.ToTable("package_versions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.PackageId, e.Version }).IsUnique();
            entity.HasIndex(e => e.PackageId);
            entity.HasIndex(e => e.LastSeenAt);
            entity.HasIndex(e => e.Version);
            entity.Property(e => e.Raw).HasColumnType("jsonb");
            entity.HasOne(e => e.Package)
                .WithMany(p => p.Versions)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IndexSyncRun>(entity =>
        {
            entity.ToTable("index_sync_runs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DiffSummary).HasColumnType("jsonb");
        });

        modelBuilder.Entity<UpdateEvent>(entity =>
        {
            entity.ToTable("update_events");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasOne(e => e.Package)
                .WithMany()
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PackageVersion)
                .WithMany()
                .HasForeignKey(e => e.PackageVersionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PublishRequest>(entity =>
        {
            entity.ToTable("publish_requests");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            entity.Property(e => e.Targets).HasColumnType("jsonb");
            entity.Property(e => e.TargetsNormalized).HasColumnType("jsonb");
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Package)
                .WithMany()
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PackageVersion)
                .WithMany()
                .HasForeignKey(e => e.PackageVersionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PublishJob>(entity =>
        {
            entity.ToTable("publish_jobs");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasOne(e => e.PublishRequest)
                .WithMany()
                .HasForeignKey(e => e.PublishRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IntuneApp>(entity =>
        {
            entity.ToTable("intune_apps");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IntuneAppId).IsUnique();
            entity.HasIndex(e => new { e.TenantId, e.PackageId }).IsUnique();
        });

        modelBuilder.Entity<AppAssignment>(entity =>
        {
            entity.ToTable("app_assignments");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.IntuneAppId, e.TargetType, e.TargetId, e.Intent }).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Payload).HasColumnType("jsonb");
        });

        modelBuilder.Entity<PublishJob>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<PublishJob>()
            .Property(e => e.Step)
            .HasConversion<int>();

        modelBuilder.Entity<PublishRequest>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<UpdateEvent>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<PublishRequest>()
            .Property(e => e.Intent)
            .HasConversion<int>();

        modelBuilder.Entity<AppAssignment>()
            .Property(e => e.Intent)
            .HasConversion<int>();

        modelBuilder.Entity<AppAssignment>()
            .Property(e => e.TargetType)
            .HasConversion<int>();
    }
}
