"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

type NavLinkProps = {
  href: string;
  label: string;
};

export function NavLink({ href, label }: NavLinkProps) {
  const pathname = usePathname();
  const isActive = pathname === href;

  return (
    <Link
      href={href}
      className={`rounded-xl px-3 py-2 text-sm font-medium transition ${
        isActive
          ? "bg-slate-900 text-white"
          : "text-slate-600 hover:bg-slate-200/70 hover:text-slate-900"
      }`}
    >
      {label}
    </Link>
  );
}
