const settings = [
  {
    label: "Notification mailbox",
    value: "intune-appbot@contoso.com"
  },
  {
    label: "Recipients",
    value: "admin@contoso.com · ops@contoso.com"
  },
  {
    label: "Sync interval",
    value: "Every 6 hours"
  }
];

export default function SettingsPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">Settings</p>
        <h1 className="text-2xl font-semibold text-slate-900">Tenant configuration</h1>
        <p className="text-sm text-slate-500">
          Manage Entra credentials, notification preferences, and sync cadence.
        </p>
      </header>

      <section className="grid gap-4 lg:grid-cols-[1.2fr_0.8fr]">
        <div className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
          <h2 className="text-sm font-semibold text-slate-900">Connection details</h2>
          <div className="mt-4 space-y-3 text-sm text-slate-600">
            {settings.map((item) => (
              <div key={item.label} className="flex items-center justify-between gap-4">
                <span className="text-slate-500">{item.label}</span>
                <span className="font-medium text-slate-900">{item.value}</span>
              </div>
            ))}
          </div>
        </div>

        <div className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
          <h2 className="text-sm font-semibold text-slate-900">Actions</h2>
          <div className="mt-4 space-y-3">
            <button
              type="button"
              className="w-full rounded-xl border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-100"
            >
              Update notification list
            </button>
            <button
              type="button"
              className="w-full rounded-xl border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-100"
            >
              Rotate client secret
            </button>
            <button
              type="button"
              className="w-full rounded-xl bg-slate-900 px-4 py-2 text-sm font-medium text-white transition hover:bg-slate-800"
            >
              Save changes
            </button>
          </div>
        </div>
      </section>
    </div>
  );
}
