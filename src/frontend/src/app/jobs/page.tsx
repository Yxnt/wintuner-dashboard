import { DataTable } from "@/components/DataTable";
import { PageHeader } from "@/components/PageHeader";
import { StatusBadge } from "@/components/StatusBadge";
import { fetchJson } from "@/lib/api";
import { formatDateTime } from "@/lib/format";
import type { JobItem } from "@/lib/types";

export default async function JobsPage() {
  const jobs = await fetchJson<JobItem[]>("/api/jobs");

  return (
    <div className="space-y-6">
      <PageHeader
        title="Jobs"
        description="Hangfire job progress for publish workflows."
      />
      <DataTable
        columns={["Package", "Step", "Status", "Progress", "Started", "Completed"]}
        rows={jobs.map((job) => [
          <div key={job.id} className="space-y-1">
            <div className="font-semibold text-slate-900">{job.packageName}</div>
            <div className="text-xs text-slate-500">v{job.version}</div>
          </div>,
          job.step,
          <StatusBadge key={`${job.id}-status`} label={job.status} />,
          `${job.progress}%`,
          <div key={`${job.id}-started`} className="text-xs text-slate-500">
            {formatDateTime(job.startedAt)}
          </div>,
          <div key={`${job.id}-completed`} className="text-xs text-slate-500">
            {job.completedAt ? formatDateTime(job.completedAt) : "-"}
          </div>
        ])}
        emptyMessage="No job activity yet."
      />
    </div>
  );
}
