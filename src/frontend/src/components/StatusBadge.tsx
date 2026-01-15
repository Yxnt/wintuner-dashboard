type StatusBadgeProps = {
  label: string;
};

const STATUS_STYLES: Record<string, string> = {
  pending: "bg-amber-100 text-amber-700",
  approved: "bg-emerald-100 text-emerald-700",
  running: "bg-blue-100 text-blue-700",
  failed: "bg-rose-100 text-rose-700",
  succeeded: "bg-emerald-100 text-emerald-700",
  "update available": "bg-amber-100 text-amber-700",
  published: "bg-emerald-100 text-emerald-700",
  "pending review": "bg-amber-100 text-amber-700"
};

const DEFAULT_STYLE = "bg-slate-100 text-slate-600";

export function StatusBadge({ label }: StatusBadgeProps) {
  const key = label.toLowerCase();
  const style = STATUS_STYLES[key] ?? DEFAULT_STYLE;

  return (
    <span className={`inline-flex items-center rounded-full px-2.5 py-1 text-xs font-semibold ${style}`}>
      {label}
    </span>
  );
}
