const jobs = [
  {
    id: "JB-2402-001",
    package: "Microsoft.PowerToys",
    step: "Upload to Intune",
    status: "Running",
    updated: "2 minutes ago"
  },
  {
    id: "JB-2402-002",
    package: "Mozilla.Firefox",
    step: "Create assignments",
    status: "Queued",
    updated: "5 minutes ago"
  },
  {
    id: "JB-2402-003",
    package: "7zip.7zip",
    step: "Detection script",
    status: "Succeeded",
    updated: "1 hour ago"
  }
];

export default function JobsPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">Jobs</p>
        <h1 className="text-2xl font-semibold text-slate-900">Publish pipeline</h1>
        <p className="text-sm text-slate-500">
          Monitor background jobs and drill into step-level status.
        </p>
      </header>

      <section className="grid gap-4">
        {jobs.map((job) => (
          <div
            key={job.id}
            className="flex flex-wrap items-center justify-between gap-4 rounded-2xl border border-slate-200 bg-white px-4 py-4 shadow-sm"
          >
            <div>
              <p className="text-xs font-semibold uppercase tracking-[0.2em] text-slate-400">{job.id}</p>
              <p className="text-sm font-semibold text-slate-900">{job.package}</p>
              <p className="text-xs text-slate-500">{job.step}</p>
            </div>
            <div className="rounded-full bg-slate-100 px-3 py-1 text-xs font-medium text-slate-600">
              {job.status}
            </div>
            <div className="text-xs text-slate-400">Updated {job.updated}</div>
          </div>
        ))}
      </section>
    </div>
  );
}
