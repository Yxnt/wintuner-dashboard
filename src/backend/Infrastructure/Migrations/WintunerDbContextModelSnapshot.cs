using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WintunerDashboard.Infrastructure.Persistence;

namespace WintunerDashboard.Infrastructure.Migrations;

[DbContext(typeof(WintunerDbContext))]
public class WintunerDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.8");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.Tenant", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("Name");
            b.Property<string>("EntraTenantId");
            b.Property<string>("GraphClientId");
            b.Property<string>("GraphAuthMode");
            b.Property<string?>("GraphSecretEncrypted");
            b.Property<string?>("CertificateThumbprint");
            b.Property<string>("NotificationMailbox");
            b.Property<string>("NotificationRecipients").HasColumnType("jsonb");
            b.Property<int>("SyncIntervalMinutes").HasDefaultValue(300);
            b.Property<DateTime>("CreatedAt");
            b.HasKey("Id");
            b.ToTable("tenants");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.Package", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("PackageIdentifier");
            b.Property<string?>("Moniker");
            b.Property<string>("DisplayName");
            b.Property<string>("Publisher");
            b.Property<string>("Tags").HasColumnType("jsonb");
            b.Property<string>("Source");
            b.Property<DateTime>("CreatedAt");
            b.Property<DateTime>("UpdatedAt");
            b.HasKey("Id");
            b.HasIndex("PackageIdentifier").IsUnique();
            b.HasIndex("DisplayName");
            b.HasIndex("Publisher");
            b.HasIndex("UpdatedAt");
            b.ToTable("packages");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PackageVersion", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("PackageId");
            b.Property<string>("Version");
            b.Property<string>("InstallerUrl");
            b.Property<string>("InstallerSha256");
            b.Property<string?>("ManifestUrl");
            b.Property<string>("Raw").HasColumnType("jsonb");
            b.Property<DateTime>("FirstSeenAt");
            b.Property<DateTime>("LastSeenAt");
            b.HasKey("Id");
            b.HasIndex("PackageId", "Version").IsUnique();
            b.HasIndex("PackageId");
            b.HasIndex("LastSeenAt");
            b.HasIndex("Version");
            b.ToTable("package_versions");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.IndexSyncRun", b =>
        {
            b.Property<Guid>("Id");
            b.Property<DateTime>("StartedAt");
            b.Property<DateTime?>("FinishedAt");
            b.Property<string?>("ETag");
            b.Property<string>("Status");
            b.Property<string>("DiffSummary").HasColumnType("jsonb");
            b.Property<string?>("Error");
            b.HasKey("Id");
            b.ToTable("index_sync_runs");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.UpdateEvent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("PackageId");
            b.Property<Guid>("PackageVersionId");
            b.Property<string>("EventType");
            b.Property<int>("Status");
            b.Property<DateTime>("CreatedAt");
            b.Property<DateTime?>("HandledAt");
            b.Property<string?>("HandledBy");
            b.Property<string?>("Note");
            b.HasKey("Id");
            b.HasIndex("Status");
            b.HasIndex("CreatedAt");
            b.ToTable("update_events");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishRequest", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("TenantId");
            b.Property<Guid>("PackageId");
            b.Property<Guid>("PackageVersionId");
            b.Property<int>("Intent");
            b.Property<string>("Targets").HasColumnType("jsonb");
            b.Property<string>("TargetsNormalized").HasColumnType("jsonb");
            b.Property<int>("Status");
            b.Property<string>("RequestedBy");
            b.Property<string?>("ApprovedBy");
            b.Property<string?>("ApprovalNote");
            b.Property<string>("IdempotencyKey");
            b.Property<DateTime>("CreatedAt");
            b.Property<DateTime>("UpdatedAt");
            b.HasKey("Id");
            b.HasIndex("Status");
            b.HasIndex("CreatedAt");
            b.HasIndex("IdempotencyKey").IsUnique();
            b.ToTable("publish_requests");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishJob", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("PublishRequestId");
            b.Property<string?>("HangfireJobId");
            b.Property<int>("Status");
            b.Property<int>("Step");
            b.Property<int>("Attempt").HasDefaultValue(1);
            b.Property<string>("Logs");
            b.Property<string?>("Error");
            b.Property<DateTime>("CreatedAt");
            b.Property<DateTime?>("StartedAt");
            b.Property<DateTime?>("FinishedAt");
            b.HasKey("Id");
            b.HasIndex("Status");
            b.HasIndex("CreatedAt");
            b.ToTable("publish_jobs");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.IntuneApp", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("TenantId");
            b.Property<Guid>("PackageId");
            b.Property<string>("IntuneAppId");
            b.Property<string>("AppName");
            b.Property<string>("CurrentVersion");
            b.Property<DateTime?>("LastPublishedAt");
            b.HasKey("Id");
            b.HasIndex("IntuneAppId").IsUnique();
            b.HasIndex("TenantId", "PackageId").IsUnique();
            b.ToTable("intune_apps");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.AppAssignment", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("TenantId");
            b.Property<string>("IntuneAppId");
            b.Property<int>("TargetType");
            b.Property<string>("TargetId");
            b.Property<int>("Intent");
            b.Property<string?>("GraphAssignmentId");
            b.Property<DateTime>("CreatedAt");
            b.HasKey("Id");
            b.HasIndex("TenantId", "IntuneAppId", "TargetType", "TargetId", "Intent").IsUnique();
            b.ToTable("app_assignments");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.AuditLog", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("ActorOid");
            b.Property<string>("Action");
            b.Property<string>("EntityType");
            b.Property<string>("EntityId");
            b.Property<string>("Payload").HasColumnType("jsonb");
            b.Property<DateTime>("CreatedAt");
            b.HasKey("Id");
            b.ToTable("audit_logs");
        });

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PackageVersion")
            .HasOne("WintunerDashboard.Domain.Entities.Package", "Package")
            .WithMany("Versions")
            .HasForeignKey("PackageId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.UpdateEvent")
            .HasOne("WintunerDashboard.Domain.Entities.Package", "Package")
            .WithMany()
            .HasForeignKey("PackageId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.UpdateEvent")
            .HasOne("WintunerDashboard.Domain.Entities.PackageVersion", "PackageVersion")
            .WithMany()
            .HasForeignKey("PackageVersionId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishRequest")
            .HasOne("WintunerDashboard.Domain.Entities.Tenant", "Tenant")
            .WithMany()
            .HasForeignKey("TenantId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishRequest")
            .HasOne("WintunerDashboard.Domain.Entities.Package", "Package")
            .WithMany()
            .HasForeignKey("PackageId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishRequest")
            .HasOne("WintunerDashboard.Domain.Entities.PackageVersion", "PackageVersion")
            .WithMany()
            .HasForeignKey("PackageVersionId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity("WintunerDashboard.Domain.Entities.PublishJob")
            .HasOne("WintunerDashboard.Domain.Entities.PublishRequest", "PublishRequest")
            .WithMany()
            .HasForeignKey("PublishRequestId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
