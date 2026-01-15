import { DataTable } from "@/components/DataTable";
import { PageHeader } from "@/components/PageHeader";
import { PublishRequestForm } from "@/components/PublishRequestForm";
import { StatusBadge } from "@/components/StatusBadge";
import { fetchJson } from "@/lib/api";
import { formatDateTime } from "@/lib/format";
import type {
  PackageItem,
  PublishRequestItem,
  SettingsResponse
} from "@/lib/types";

export default async function PublishRequestsPage() {
  const [requests, settings, packages] = await Promise.all([
    fetchJson<PublishRequestItem[]>("/api/publish-requests"),
    fetchJson<SettingsResponse>("/api/settings"),
    fetchJson<PackageItem[]>("/api/packages")
  ]);

  return (
    <div className="space-y-6">
      <PageHeader
        title="Publish requests"
        description="Submit requests for new Win32 app publishes or assignments."
      />
      <PublishRequestForm
        packages={packages}
        targets={settings.availableGroups}
        defaultTarget={settings.defaultTarget}
      />
      <DataTable
        columns={["Package", "Target", "Intent", "Status", "Requested by"]}
        rows={requests.map((request) => [
          <div key={request.id} className="space-y-1">
            <div className="font-semibold text-slate-900">
              {request.packageName}
            </div>
            <div className="text-xs text-slate-500">v{request.version}</div>
          </div>,
          request.target,
          request.intent,
          <StatusBadge key={`${request.id}-status`} label={request.status} />,
          <div key={`${request.id}-requested`} className="text-xs text-slate-500">
            {request.requestedBy}
            <div>{formatDateTime(request.requestedAt)}</div>
          </div>
        ])}
        emptyMessage="No publish requests created yet."
      />
    </div>
  );
}
