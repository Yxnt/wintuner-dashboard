import { DataTable } from "@/components/DataTable";
import { PageHeader } from "@/components/PageHeader";
import { StatusBadge } from "@/components/StatusBadge";
import { fetchJson } from "@/lib/api";
import { formatDate } from "@/lib/format";
import type { PackageItem } from "@/lib/types";

export default async function PackagesPage() {
  const packages = await fetchJson<PackageItem[]>("/api/packages");

  return (
    <div className="space-y-6">
      <PageHeader
        title="Packages"
        description="Track WinGet packages synced for Intune publishing."
      />
      <DataTable
        columns={["Package", "Publisher", "Version", "Status", "Updated"]}
        rows={packages.map((pkg) => [
          <div key={pkg.id} className="space-y-1">
            <div className="font-semibold text-slate-900">{pkg.name}</div>
            <div className="text-xs text-slate-500">{pkg.id}</div>
          </div>,
          pkg.publisher,
          <div key={`${pkg.id}-version`} className="text-xs text-slate-500">
            v{pkg.currentVersion}
            <div className="text-[11px] text-slate-400">
              {pkg.versions.length} versions
            </div>
          </div>,
          <StatusBadge key={`${pkg.id}-status`} label={pkg.status} />,
          <div key={`${pkg.id}-updated`} className="text-xs text-slate-500">
            {formatDate(pkg.lastUpdated)}
          </div>
        ])}
        emptyMessage="No packages synced yet."
      />
    </div>
  );
}
