import { DataTable } from "@/components/DataTable";
import { PageHeader } from "@/components/PageHeader";
import { StatusBadge } from "@/components/StatusBadge";
import { fetchJson } from "@/lib/api";
import { formatDateTime } from "@/lib/format";
import type { UpdateEventItem } from "@/lib/types";

export default async function UpdatesPage() {
  const updates = await fetchJson<UpdateEventItem[]>("/api/updates");

  return (
    <div className="space-y-6">
      <PageHeader
        title="Updates"
        description="Monitor incoming package updates and approval status."
      />
      <DataTable
        columns={[
          "Package",
          "Current",
          "Latest",
          "Status",
          "Severity",
          "Detected"
        ]}
        rows={updates.map((update) => [
          <div key={update.id} className="space-y-1">
            <div className="font-semibold text-slate-900">
              {update.packageName}
            </div>
            <div className="text-xs text-slate-500">Owner: {update.actionOwner}</div>
          </div>,
          update.currentVersion,
          update.latestVersion,
          <StatusBadge key={`${update.id}-status`} label={update.status} />,
          <StatusBadge key={`${update.id}-severity`} label={update.severity} />,
          <div key={`${update.id}-detected`} className="text-xs text-slate-500">
            {formatDateTime(update.detectedAt)}
          </div>
        ])}
        emptyMessage="No updates detected yet."
      />
    </div>
  );
}
