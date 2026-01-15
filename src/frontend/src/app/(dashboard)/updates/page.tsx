const updates = [
  {
    package: "Microsoft.PowerToys",
    version: "0.87.1",
    status: "Pending approval",
    detected: "10 minutes ago"
  },
  {
    package: "Mozilla.Firefox",
    version: "123.0",
    status: "Drafted",
    detected: "45 minutes ago"
  },
  {
    package: "7zip.7zip",
    version: "24.00",
    status: "Notified",
    detected: "2 hours ago"
  }
];

export default function UpdatesPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">Updates</p>
        <h1 className="text-2xl font-semibold text-slate-900">Pending updates</h1>
        <p className="text-sm text-slate-500">
          Track new versions detected in the WinGet index and decide whether to publish.
        </p>
      </header>

      <section className="grid gap-4">
        {updates.map((item) => (
          <div
            key={`${item.package}-${item.version}`}
            className="flex flex-wrap items-center justify-between gap-4 rounded-2xl border border-slate-200 bg-white px-4 py-4 shadow-sm"
          >
            <div>
              <p className="text-sm font-semibold text-slate-900">{item.package}</p>
              <p className="text-xs text-slate-500">Version {item.version}</p>
            </div>
            <div className="rounded-full bg-slate-100 px-3 py-1 text-xs font-medium text-slate-600">
              {item.status}
            </div>
            <div className="text-xs text-slate-400">Detected {item.detected}</div>
          </div>
        ))}
      </section>
    </div>
  );
}
