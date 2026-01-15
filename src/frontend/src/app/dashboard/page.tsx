import { DataTable } from "@/components/DataTable";
import { PageHeader } from "@/components/PageHeader";
import { StatCard } from "@/components/StatCard";
import { StatusBadge } from "@/components/StatusBadge";
import { fetchJson } from "@/lib/api";
import { formatDateTime } from "@/lib/format";
import type { DashboardSummary } from "@/lib/types";

export default async function DashboardPage() {
  const summary = await fetchJson<DashboardSummary>("/api/dashboard");

  return (
    <div className="space-y-8">
      <PageHeader
        title="Dashboard"
        description="Snapshot of packages, publish requests, and job activity."
      />
      <section className="grid gap-4 sm:grid-cols-2 lg:grid-cols-5">
        <StatCard label="Total packages" value={summary.stats.totalPackages} />
        <StatCard
          label="Pending requests"
          value={summary.stats.pendingPublishRequests}
        />
        <StatCard label="Running jobs" value={summary.stats.runningJobs} />
        <StatCard label="Failed jobs" value={summary.stats.failedJobs} />
        <StatCard label="Published apps" value={summary.stats.publishedApps} />
      </section>
      <section className="grid gap-6 lg:grid-cols-2">
        <div className="space-y-3">
          <h3 className="text-lg font-semibold text-slate-900">
            Recent publish requests
          </h3>
          <DataTable
            columns={["Package", "Target", "Intent", "Status", "Requested"]}
            rows={summary.recentPublishRequests.map((request) => [
              <div key={request.id} className="space-y-1">
                <div className="font-semibold text-slate-900">
                  {request.packageName}
                </div>
                <div className="text-xs text-slate-500">v{request.version}</div>
              </div>,
              request.target,
              request.intent,
              <StatusBadge key={`${request.id}-status`} label={request.status} />,
              <div key={`${request.id}-time`} className="text-xs text-slate-500">
                {formatDateTime(request.requestedAt)}
              </div>
            ])}
            emptyMessage="No publish requests yet."
          />
        </div>
        <div className="space-y-3">
          <h3 className="text-lg font-semibold text-slate-900">Active jobs</h3>
          <DataTable
            columns={["Package", "Step", "Status", "Progress", "Started"]}
            rows={summary.recentJobs.map((job) => [
              <div key={job.id} className="space-y-1">
                <div className="font-semibold text-slate-900">
                  {job.packageName}
                </div>
                <div className="text-xs text-slate-500">v{job.version}</div>
              </div>,
              job.step,
              <StatusBadge key={`${job.id}-status`} label={job.status} />,
              `${job.progress}%`,
              <div key={`${job.id}-time`} className="text-xs text-slate-500">
                {formatDateTime(job.startedAt)}
              </div>
            ])}
            emptyMessage="No jobs running right now."
          />
        </div>
      </section>
    </div>
  );
}
