const requests = [
  {
    package: "Microsoft.PowerToys",
    version: "0.87.1",
    intent: "Available",
    status: "Approved",
    requestedBy: "alex@contoso.com"
  },
  {
    package: "Mozilla.Firefox",
    version: "123.0",
    intent: "Required",
    status: "Pending approval",
    requestedBy: "sam@contoso.com"
  },
  {
    package: "7zip.7zip",
    version: "24.00",
    intent: "Available",
    status: "Draft",
    requestedBy: "riley@contoso.com"
  }
];

export default function PublishRequestsPage() {
  return (
    <div className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">
          Publish requests
        </p>
        <h1 className="text-2xl font-semibold text-slate-900">Approval queue</h1>
        <p className="text-sm text-slate-500">
          Review new publish requests before creating Intune assignments.
        </p>
      </header>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white">
        <div className="grid grid-cols-[1.4fr_0.6fr_0.6fr_0.8fr_1fr] gap-4 border-b border-slate-100 px-4 py-3 text-xs font-semibold uppercase tracking-[0.2em] text-slate-400">
          <span>Package</span>
          <span>Version</span>
          <span>Intent</span>
          <span>Status</span>
          <span>Requested by</span>
        </div>
        <div className="divide-y divide-slate-100">
          {requests.map((item) => (
            <div
              key={`${item.package}-${item.version}`}
              className="grid grid-cols-[1.4fr_0.6fr_0.6fr_0.8fr_1fr] items-center gap-4 px-4 py-3 text-sm text-slate-700"
            >
              <span className="font-medium text-slate-900">{item.package}</span>
              <span>{item.version}</span>
              <span>{item.intent}</span>
              <span className="text-slate-600">{item.status}</span>
              <span>{item.requestedBy}</span>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}
