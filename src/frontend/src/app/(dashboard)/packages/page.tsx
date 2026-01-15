const packages = [
  {
    name: "Microsoft.PowerToys",
    publisher: "Microsoft",
    version: "0.87.1",
    updated: "2024-02-01"
  },
  {
    name: "Mozilla.Firefox",
    publisher: "Mozilla",
    version: "123.0",
    updated: "2024-02-01"
  },
  {
    name: "7zip.7zip",
    publisher: "Igor Pavlov",
    version: "24.00",
    updated: "2024-01-30"
  },
  {
    name: "Microsoft.VisualStudioCode",
    publisher: "Microsoft",
    version: "1.86.2",
    updated: "2024-01-29"
  }
];

export default function PackagesPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">Packages</p>
        <h1 className="text-2xl font-semibold text-slate-900">Tracked packages</h1>
        <p className="text-sm text-slate-500">
          Review the latest WinGet metadata and prepare publish requests.
        </p>
      </header>

      <div className="flex flex-wrap gap-3">
        <button
          type="button"
          className="rounded-xl border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-100"
        >
          Sync index
        </button>
        <button
          type="button"
          className="rounded-xl bg-slate-900 px-4 py-2 text-sm font-medium text-white transition hover:bg-slate-800"
        >
          New publish request
        </button>
      </div>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white">
        <div className="grid grid-cols-[1.6fr_1fr_0.6fr_0.8fr] gap-4 border-b border-slate-100 px-4 py-3 text-xs font-semibold uppercase tracking-[0.2em] text-slate-400">
          <span>Package</span>
          <span>Publisher</span>
          <span>Version</span>
          <span>Updated</span>
        </div>
        <div className="divide-y divide-slate-100">
          {packages.map((item) => (
            <div
              key={item.name}
              className="grid grid-cols-[1.6fr_1fr_0.6fr_0.8fr] items-center gap-4 px-4 py-3 text-sm text-slate-700"
            >
              <span className="font-medium text-slate-900">{item.name}</span>
              <span>{item.publisher}</span>
              <span>{item.version}</span>
              <span>{item.updated}</span>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}
