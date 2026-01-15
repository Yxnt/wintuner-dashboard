type PageHeaderProps = {
  title: string;
  description?: string;
  action?: React.ReactNode;
};

export function PageHeader({ title, description, action }: PageHeaderProps) {
  return (
    <div className="flex flex-wrap items-center justify-between gap-4">
      <div className="space-y-1">
        <h2 className="text-2xl font-semibold text-slate-900">{title}</h2>
        {description ? (
          <p className="text-sm text-slate-600">{description}</p>
        ) : null}
      </div>
      {action ? <div>{action}</div> : null}
    </div>
  );
}
