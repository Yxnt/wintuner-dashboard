type StatCardProps = {
  label: string;
  value: number | string;
  helper?: string;
};

export function StatCard({ label, value, helper }: StatCardProps) {
  return (
    <div className="rounded-2xl border border-slate-200/70 bg-white px-5 py-4 shadow-sm">
      <p className="text-xs font-semibold uppercase tracking-[0.2em] text-slate-500">
        {label}
      </p>
      <div className="mt-3 text-2xl font-semibold text-slate-900">{value}</div>
      {helper ? <p className="mt-1 text-xs text-slate-500">{helper}</p> : null}
    </div>
  );
}
