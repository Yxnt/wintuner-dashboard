# WinTuner Dashboard (Intune Win32 publisher for WinGet)

> **Status:** Project scaffold + architecture blueprint. Implementation will follow the ordered plan below.

## Architecture & Module Overview (Phase 0)

```
repo/
  src/
    backend/
      WebApi/
      Application/
      Domain/
      Infrastructure/
      Worker/
      Tests/
    frontend/
  infra/
    docker-compose.yml
  README.md
  .env.example
```

### Backend modules

- **Domain**
  - Entities (tenants, packages, package_versions, index_sync_runs, update_events, publish_requests, publish_jobs, intune_apps, app_assignments, audit_logs).
  - Value objects (Target, TargetType, InstallIntent, Version, IdempotencyKey).
  - Enums (PublishRequestStatus, UpdateEventStatus, PublishJobStatus, PublishJobStep).
  - Domain services: `PublishRequestStateMachine`, `VersionComparator`.
- **Application**
  - Use-cases / command handlers for sync, approvals, publish request flow, publish job run.
  - Validators (FluentValidation): request payloads, status transitions.
  - Idempotency & normalization: `TargetsNormalizer`, `IdempotencyKeyBuilder`.
  - DTOs and pagination models.
- **Infrastructure**
  - EF Core (Npgsql) DbContext + migrations.
  - Hangfire (PostgreSQL storage) background job orchestration.
  - Graph client wrappers (`IGraphClient`, `IIntuneAppService`, `IAssignmentService`, `IUserGroupDirectoryService`).
  - WinTuner integration via `IWinTunerService` (Download, VerifySha256, BuildIntuneWin, GenerateDetectionScript).
  - Mail via Microsoft Graph `sendMail` to the configured mailbox.
- **WebApi**
  - ASP.NET Core Web API, Swagger/OpenAPI, Serilog.
  - REST endpoints listed in the requirements.
  - AuthN/AuthZ with Entra JWT, role mapping with group membership.
- **Worker**
  - Hangfire server (can be hosted in WebApi for dev or separated).
- **Tests**
  - Unit tests for diff logic, idempotency key, status machine, SHA256, version comparison.

### Frontend modules

- **App Router** (Next.js) with shadcn/ui + Tailwind.
- Pages: `/dashboard`, `/packages`, `/updates`, `/publish-requests`, `/jobs`, `/settings`.
- Shared components: `DataTable`, `Pagination`, `TargetPicker`, `PublishRequestForm`, `JobSteps`, `Toast`, `ConfirmDialog`.

## Key hard constraints (enforced in DB + service logic)

- **Single Intune App per (tenant + package_identifier).**
- **Multi assignment per app** (Available/Required) based on target.
- **Idempotent publish**: avoid duplicate assignments for same `(tenant, package_identifier, version, target, intent)`.
- **Installer SHA256 verification** against index/manifest before publish.
- **Audit logs** for all write operations.
- **All background jobs** are step-tracked, retryable, and recoverable.

## Entra App Registrations (two-app model)

### A) SPA (Frontend)
- Platform: **Single-page application**
- Permissions: `openid`, `profile`, `email`, `offline_access`
- Calls backend API using a custom scope (recommended) or cookie session.

### B) Backend / Worker (Confidential client)
- Platform: **Web / Confidential**
- Auth: client secret or certificate (preferred).
- Uses **Application permissions** for Graph.

## Microsoft Graph permissions (Application)

- **DeviceManagementApps.ReadWrite.All** (Win32 App + assignment)
- **Group.Read.All**
- **User.Read.All**
- **Mail.Send** (notification mailbox only)

> Security requirement: configure Exchange Online **Application Access Policy** so the app can only send mail as the configured notification mailbox. All code must send with `/users/{notification_mailbox}/sendMail`.

## Implementation order

1) **Architecture doc** (this file) ✅
2) **DB + migrations**
3) **Backend API + Hangfire jobs + WinTuner service + Graph service**
4) **Frontend pages**
5) **README + docker-compose + tests**

## Validated on Next.js

> Pending: use `npx create-next-app@latest` to initialize the frontend, then record the exact version from `next --version` here.

## Environment

- Node.js >= 20.9
- .NET 8 SDK
- PostgreSQL

## Quick start (dev)

1. Copy `.env.example` and fill in secrets.
2. Start with Docker Compose.

```
cd infra
# docker compose up --build
```

## Definition of Done (demo steps)

1. Trigger index sync
2. Create publish request for a package + version (Available)
3. Approve + run
4. Observe job steps
5. Create Required assignment for same app/target (no duplicate app)
6. Verify idempotency (repeat run)
7. New version triggers pending update and email notification
8. Audit logs show all operations
