const stats = [
  { label: "Packages tracked", value: "214" },
  { label: "Pending updates", value: "7" },
  { label: "Publish requests", value: "12" },
  { label: "Active jobs", value: "3" }
];

const activity = [
  {
    title: "WinGet index sync completed",
    description: "216 packages updated · 2 new versions discovered",
    time: "12 minutes ago"
  },
  {
    title: "Publish request approved",
    description: "Microsoft.PowerToys 0.87.1 → Available",
    time: "38 minutes ago"
  },
  {
    title: "Assignment created",
    description: "Required deployment to Finance Devices",
    time: "1 hour ago"
  }
];

export default function DashboardPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">
          Dashboard
        </p>
        <h1 className="text-2xl font-semibold text-slate-900">WinTuner overview</h1>
        <p className="text-sm text-slate-500">
          Monitor the index sync, publish requests, and Intune deployments in one place.
        </p>
      </header>

      <section className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {stats.map((item) => (
          <div
            key={item.label}
            className="rounded-2xl border border-slate-200 bg-white px-4 py-5 shadow-sm"
          >
            <p className="text-xs uppercase tracking-[0.2em] text-slate-400">{item.label}</p>
            <p className="mt-3 text-2xl font-semibold text-slate-900">{item.value}</p>
          </div>
        ))}
      </section>

      <section className="grid gap-4 lg:grid-cols-[1.2fr_0.8fr]">
        <div className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
          <h2 className="text-sm font-semibold text-slate-900">Recent activity</h2>
          <div className="mt-4 flex flex-col gap-4">
            {activity.map((item) => (
              <div key={item.title} className="rounded-2xl border border-slate-100 bg-slate-50 px-4 py-3">
                <p className="text-sm font-medium text-slate-900">{item.title}</p>
                <p className="text-xs text-slate-500">{item.description}</p>
                <p className="mt-2 text-xs text-slate-400">{item.time}</p>
              </div>
            ))}
          </div>
        </div>

        <div className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
          <h2 className="text-sm font-semibold text-slate-900">Next scheduled tasks</h2>
          <ul className="mt-4 space-y-3 text-sm text-slate-600">
            <li className="rounded-xl border border-slate-100 bg-slate-50 px-3 py-2">
              Index sync · Every 6 hours
            </li>
            <li className="rounded-xl border border-slate-100 bg-slate-50 px-3 py-2">
              Update scan · Every 2 hours
            </li>
            <li className="rounded-xl border border-slate-100 bg-slate-50 px-3 py-2">
              Notifications · Instant
            </li>
          </ul>
          <button
            type="button"
            className="mt-4 w-full rounded-xl border border-slate-200 bg-white px-3 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-100"
          >
            Configure schedules
          </button>
        </div>
      </section>
    </div>
  );
}
