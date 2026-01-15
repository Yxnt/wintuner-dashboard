type KeyValueItem = {
  label: string;
  value: string | React.ReactNode;
};

type KeyValueListProps = {
  items: KeyValueItem[];
};

export function KeyValueList({ items }: KeyValueListProps) {
  return (
    <dl className="grid gap-4 rounded-2xl border border-slate-200/70 bg-white p-6 text-sm text-slate-700 shadow-sm">
      {items.map((item) => (
        <div key={item.label} className="space-y-1">
          <dt className="text-xs font-semibold uppercase tracking-[0.2em] text-slate-500">
            {item.label}
          </dt>
          <dd className="text-base font-medium text-slate-900">{item.value}</dd>
        </div>
      ))}
    </dl>
  );
}
