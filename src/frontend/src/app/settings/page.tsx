import { KeyValueList } from "@/components/KeyValueList";
import { PageHeader } from "@/components/PageHeader";
import { fetchJson } from "@/lib/api";
import type { SettingsResponse } from "@/lib/types";

export default async function SettingsPage() {
  const settings = await fetchJson<SettingsResponse>("/api/settings");

  return (
    <div className="space-y-6">
      <PageHeader
        title="Settings"
        description="Configuration details pulled from the backend."
      />
      <KeyValueList
        items={[
          { label: "Tenant", value: settings.tenantName },
          { label: "Environment", value: settings.environment },
          { label: "Notification mailbox", value: settings.notificationMailbox },
          { label: "Default target", value: settings.defaultTarget },
          {
            label: "Available groups",
            value: settings.availableGroups.join(", ")
          }
        ]}
      />
    </div>
  );
}
