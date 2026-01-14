export default function HomePage() {
  return (
    <main className="flex min-h-screen flex-col items-center justify-center gap-6 px-6 py-12">
      <section className="w-full max-w-3xl rounded-3xl border border-slate-200/60 bg-white/70 p-8 shadow-sm backdrop-blur">
        <div className="flex flex-col gap-4">
          <span className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
            WinTuner Dashboard
          </span>
          <h1 className="text-3xl font-semibold text-slate-900 sm:text-4xl">
            Frontend scaffold is ready.
          </h1>
          <p className="text-base leading-relaxed text-slate-600">
            This Next.js App Router setup is wired for Tailwind CSS so you can start building the
            dashboard pages and shared components.
          </p>
          <div className="grid gap-3 sm:grid-cols-2">
            {[
              "Packages",
              "Updates",
              "Publish Requests",
              "Jobs",
              "Settings"
            ].map((label) => (
              <div
                key={label}
                className="rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 text-sm font-medium text-slate-700"
              >
                {label}
              </div>
            ))}
          </div>
        </div>
      </section>
    </main>
  );
}
