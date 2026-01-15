import { NavLink } from "@/components/NavLink";

type NavItem = {
  href: string;
  label: string;
};

type NavBarProps = {
  items: NavItem[];
};

export function NavBar({ items }: NavBarProps) {
  return (
    <aside className="flex w-full flex-col gap-6 border-b border-slate-200/70 bg-white/70 px-6 py-6 shadow-sm backdrop-blur lg:min-h-screen lg:w-64 lg:border-b-0 lg:border-r">
      <div className="space-y-1">
        <p className="text-xs font-semibold uppercase tracking-[0.2em] text-slate-500">
          WinTuner
        </p>
        <h1 className="text-lg font-semibold text-slate-900">Dashboard</h1>
      </div>
      <nav className="grid gap-1">
        {items.map((item) => (
          <NavLink key={item.href} href={item.href} label={item.label} />
        ))}
      </nav>
    </aside>
  );
}
