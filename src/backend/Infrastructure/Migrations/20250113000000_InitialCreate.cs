using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WintunerDashboard.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

        migrationBuilder.CreateTable(
            name: "tenants",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                EntraTenantId = table.Column<string>(type: "text", nullable: false),
                GraphClientId = table.Column<string>(type: "text", nullable: false),
                GraphAuthMode = table.Column<string>(type: "text", nullable: false),
                GraphSecretEncrypted = table.Column<string>(type: "text", nullable: true),
                CertificateThumbprint = table.Column<string>(type: "text", nullable: true),
                NotificationMailbox = table.Column<string>(type: "text", nullable: false),
                NotificationRecipients = table.Column<string>(type: "jsonb", nullable: false),
                SyncIntervalMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 300),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tenants", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "packages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PackageIdentifier = table.Column<string>(type: "text", nullable: false),
                Moniker = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: false),
                Publisher = table.Column<string>(type: "text", nullable: false),
                Tags = table.Column<string>(type: "jsonb", nullable: false),
                Source = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_packages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "index_sync_runs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ETag = table.Column<string>(type: "text", nullable: true),
                Status = table.Column<string>(type: "text", nullable: false),
                DiffSummary = table.Column<string>(type: "jsonb", nullable: false),
                Error = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_index_sync_runs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "audit_logs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ActorOid = table.Column<string>(type: "text", nullable: false),
                Action = table.Column<string>(type: "text", nullable: false),
                EntityType = table.Column<string>(type: "text", nullable: false),
                EntityId = table.Column<string>(type: "text", nullable: false),
                Payload = table.Column<string>(type: "jsonb", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_audit_logs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "package_versions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                Version = table.Column<string>(type: "text", nullable: false),
                InstallerUrl = table.Column<string>(type: "text", nullable: false),
                InstallerSha256 = table.Column<string>(type: "text", nullable: false),
                ManifestUrl = table.Column<string>(type: "text", nullable: true),
                Raw = table.Column<string>(type: "jsonb", nullable: false),
                FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_package_versions", x => x.Id);
                table.ForeignKey(
                    name: "FK_package_versions_packages_PackageId",
                    column: x => x.PackageId,
                    principalTable: "packages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "update_events",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                PackageVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                EventType = table.Column<string>(type: "text", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                HandledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                HandledBy = table.Column<string>(type: "text", nullable: true),
                Note = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_update_events", x => x.Id);
                table.ForeignKey(
                    name: "FK_update_events_packages_PackageId",
                    column: x => x.PackageId,
                    principalTable: "packages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_update_events_package_versions_PackageVersionId",
                    column: x => x.PackageVersionId,
                    principalTable: "package_versions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "publish_requests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                PackageVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                Intent = table.Column<int>(type: "integer", nullable: false),
                Targets = table.Column<string>(type: "jsonb", nullable: false),
                TargetsNormalized = table.Column<string>(type: "jsonb", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                RequestedBy = table.Column<string>(type: "text", nullable: false),
                ApprovedBy = table.Column<string>(type: "text", nullable: true),
                ApprovalNote = table.Column<string>(type: "text", nullable: true),
                IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_publish_requests", x => x.Id);
                table.ForeignKey(
                    name: "FK_publish_requests_packages_PackageId",
                    column: x => x.PackageId,
                    principalTable: "packages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_publish_requests_package_versions_PackageVersionId",
                    column: x => x.PackageVersionId,
                    principalTable: "package_versions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_publish_requests_tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "tenants",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "publish_jobs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PublishRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                HangfireJobId = table.Column<string>(type: "text", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                Step = table.Column<int>(type: "integer", nullable: false),
                Attempt = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                Logs = table.Column<string>(type: "text", nullable: false),
                Error = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_publish_jobs", x => x.Id);
                table.ForeignKey(
                    name: "FK_publish_jobs_publish_requests_PublishRequestId",
                    column: x => x.PublishRequestId,
                    principalTable: "publish_requests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "intune_apps",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                IntuneAppId = table.Column<string>(type: "text", nullable: false),
                AppName = table.Column<string>(type: "text", nullable: false),
                CurrentVersion = table.Column<string>(type: "text", nullable: false),
                LastPublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_intune_apps", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "app_assignments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                IntuneAppId = table.Column<string>(type: "text", nullable: false),
                TargetType = table.Column<int>(type: "integer", nullable: false),
                TargetId = table.Column<string>(type: "text", nullable: false),
                Intent = table.Column<int>(type: "integer", nullable: false),
                GraphAssignmentId = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_app_assignments", x => x.Id);
            });

        migrationBuilder.CreateIndex(name: "IX_packages_PackageIdentifier", table: "packages", column: "PackageIdentifier", unique: true);
        migrationBuilder.CreateIndex(name: "IX_packages_DisplayName", table: "packages", column: "DisplayName");
        migrationBuilder.CreateIndex(name: "IX_packages_Publisher", table: "packages", column: "Publisher");
        migrationBuilder.CreateIndex(name: "IX_packages_UpdatedAt", table: "packages", column: "UpdatedAt");

        migrationBuilder.CreateIndex(name: "IX_package_versions_PackageId_Version", table: "package_versions", columns: new[] { "PackageId", "Version" }, unique: true);
        migrationBuilder.CreateIndex(name: "IX_package_versions_PackageId", table: "package_versions", column: "PackageId");
        migrationBuilder.CreateIndex(name: "IX_package_versions_LastSeenAt", table: "package_versions", column: "LastSeenAt");
        migrationBuilder.CreateIndex(name: "IX_package_versions_Version", table: "package_versions", column: "Version");

        migrationBuilder.CreateIndex(name: "IX_update_events_Status", table: "update_events", column: "Status");
        migrationBuilder.CreateIndex(name: "IX_update_events_CreatedAt", table: "update_events", column: "CreatedAt");

        migrationBuilder.CreateIndex(name: "IX_publish_requests_Status", table: "publish_requests", column: "Status");
        migrationBuilder.CreateIndex(name: "IX_publish_requests_CreatedAt", table: "publish_requests", column: "CreatedAt");
        migrationBuilder.CreateIndex(name: "IX_publish_requests_IdempotencyKey", table: "publish_requests", column: "IdempotencyKey", unique: true);

        migrationBuilder.CreateIndex(name: "IX_publish_jobs_Status", table: "publish_jobs", column: "Status");
        migrationBuilder.CreateIndex(name: "IX_publish_jobs_CreatedAt", table: "publish_jobs", column: "CreatedAt");

        migrationBuilder.CreateIndex(name: "IX_intune_apps_IntuneAppId", table: "intune_apps", column: "IntuneAppId", unique: true);
        migrationBuilder.CreateIndex(name: "IX_intune_apps_TenantId_PackageId", table: "intune_apps", columns: new[] { "TenantId", "PackageId" }, unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_app_assignments_UQ",
            table: "app_assignments",
            columns: new[] { "TenantId", "IntuneAppId", "TargetType", "TargetId", "Intent" },
            unique: true);

        migrationBuilder.CreateIndex(name: "IX_update_events_PackageId", table: "update_events", column: "PackageId");
        migrationBuilder.CreateIndex(name: "IX_update_events_PackageVersionId", table: "update_events", column: "PackageVersionId");
        migrationBuilder.CreateIndex(name: "IX_publish_requests_TenantId", table: "publish_requests", column: "TenantId");
        migrationBuilder.CreateIndex(name: "IX_publish_requests_PackageId", table: "publish_requests", column: "PackageId");
        migrationBuilder.CreateIndex(name: "IX_publish_requests_PackageVersionId", table: "publish_requests", column: "PackageVersionId");
        migrationBuilder.CreateIndex(name: "IX_publish_jobs_PublishRequestId", table: "publish_jobs", column: "PublishRequestId");
        migrationBuilder.CreateIndex(name: "IX_package_versions_PackageId_1", table: "package_versions", column: "PackageId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "app_assignments");
        migrationBuilder.DropTable(name: "audit_logs");
        migrationBuilder.DropTable(name: "index_sync_runs");
        migrationBuilder.DropTable(name: "intune_apps");
        migrationBuilder.DropTable(name: "publish_jobs");
        migrationBuilder.DropTable(name: "update_events");
        migrationBuilder.DropTable(name: "publish_requests");
        migrationBuilder.DropTable(name: "package_versions");
        migrationBuilder.DropTable(name: "tenants");
        migrationBuilder.DropTable(name: "packages");
    }
}
