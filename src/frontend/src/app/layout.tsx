import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { NavBar } from "@/components/NavBar";

export const metadata: Metadata = {
  title: "WinTuner Dashboard",
  description: "Intune Win32 publisher for WinGet"
};

const inter = Inter({
  subsets: ["latin"]
});

const navItems = [
  { href: "/dashboard", label: "Dashboard" },
  { href: "/packages", label: "Packages" },
  { href: "/updates", label: "Updates" },
  { href: "/publish-requests", label: "Publish Requests" },
  { href: "/jobs", label: "Jobs" },
  { href: "/settings", label: "Settings" }
];

export default function RootLayout({
  children
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body
        className={`${inter.className} min-h-screen bg-background text-foreground antialiased`}
      >
        <div className="flex min-h-screen flex-col bg-slate-100 lg:flex-row">
          <NavBar items={navItems} />
          <main className="flex-1 px-6 py-8 lg:px-10">
            <div className="mx-auto flex w-full max-w-6xl flex-col gap-8">
              {children}
            </div>
          </main>
        </div>
      </body>
    </html>
  );
}
