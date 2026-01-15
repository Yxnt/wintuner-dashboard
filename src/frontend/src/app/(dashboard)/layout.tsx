import Link from "next/link";

const navItems = [
  { href: "/dashboard", label: "Dashboard" },
  { href: "/packages", label: "Packages" },
  { href: "/updates", label: "Updates" },
  { href: "/publish-requests", label: "Publish Requests" },
  { href: "/jobs", label: "Jobs" },
  { href: "/settings", label: "Settings" }
];

export default function DashboardLayout({
  children
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="min-h-screen bg-slate-50">
      <div className="mx-auto flex w-full max-w-6xl gap-6 px-6 py-8">
        <aside className="hidden w-56 flex-col gap-4 rounded-3xl border border-slate-200 bg-white/70 p-6 shadow-sm backdrop-blur lg:flex">
          <div className="flex flex-col gap-1">
            <span className="text-xs font-semibold uppercase tracking-[0.3em] text-slate-400">
              WinTuner
            </span>
            <span className="text-lg font-semibold text-slate-900">Publisher Console</span>
          </div>
          <nav className="flex flex-1 flex-col gap-2">
            {navItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className="rounded-xl px-3 py-2 text-sm font-medium text-slate-600 transition hover:bg-slate-100 hover:text-slate-900"
              >
                {item.label}
              </Link>
            ))}
          </nav>
          <div className="rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 text-xs text-slate-500">
            Running in preview mode · Connect to the API to see live data.
          </div>
        </aside>
        <main className="flex-1 rounded-3xl border border-slate-200 bg-white/70 p-6 shadow-sm backdrop-blur">
          {children}
        </main>
      </div>
    </div>
  );
}
