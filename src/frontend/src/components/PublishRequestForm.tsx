"use client";

import { useMemo, useState } from "react";
import { API_BASE_URL } from "@/lib/api";
import type { CreatePublishRequest, PackageItem } from "@/lib/types";

const INTENTS = ["Available", "Required"] as const;

type PublishRequestFormProps = {
  packages: PackageItem[];
  targets: string[];
  defaultTarget?: string;
};

export function PublishRequestForm({
  packages,
  targets,
  defaultTarget
}: PublishRequestFormProps) {
  const [formState, setFormState] = useState<CreatePublishRequest>({
    packageId: packages[0]?.id ?? "",
    version: packages[0]?.currentVersion ?? "",
    target: defaultTarget ?? targets[0] ?? "",
    intent: INTENTS[0],
    requestedBy: ""
  });
  const [status, setStatus] = useState<"idle" | "saving" | "success" | "error">(
    "idle"
  );

  const selectedPackage = useMemo(
    () => packages.find((pkg) => pkg.id === formState.packageId),
    [formState.packageId, packages]
  );

  function handleChange(
    field: keyof CreatePublishRequest,
    value: string
  ) {
    setFormState((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setStatus("saving");

    try {
      const response = await fetch(`${API_BASE_URL}/api/publish-requests`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(formState)
      });

      if (!response.ok) {
        throw new Error("Request failed");
      }

      setStatus("success");
      setFormState((prev) => ({
        ...prev,
        requestedBy: ""
      }));
    } catch (error) {
      setStatus("error");
    }
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="grid gap-4 rounded-2xl border border-slate-200/70 bg-white p-6 shadow-sm"
    >
      <div className="grid gap-4 lg:grid-cols-2">
        <label className="grid gap-2 text-sm font-medium text-slate-700">
          Package
          <select
            className="rounded-xl border border-slate-200 px-3 py-2 text-sm"
            value={formState.packageId}
            onChange={(event) => {
              const selected = event.target.value;
              const match = packages.find((pkg) => pkg.id === selected);
              setFormState((prev) => ({
                ...prev,
                packageId: selected,
                version: match?.currentVersion ?? prev.version
              }));
            }}
          >
            {packages.map((pkg) => (
              <option key={pkg.id} value={pkg.id}>
                {pkg.name}
              </option>
            ))}
          </select>
        </label>
        <label className="grid gap-2 text-sm font-medium text-slate-700">
          Version
          <input
            className="rounded-xl border border-slate-200 px-3 py-2 text-sm"
            value={formState.version}
            onChange={(event) => handleChange("version", event.target.value)}
            placeholder="1.0.0"
          />
          {selectedPackage ? (
            <span className="text-xs text-slate-500">
              Current: {selectedPackage.currentVersion}
            </span>
          ) : null}
        </label>
        <label className="grid gap-2 text-sm font-medium text-slate-700">
          Target
          <select
            className="rounded-xl border border-slate-200 px-3 py-2 text-sm"
            value={formState.target}
            onChange={(event) => handleChange("target", event.target.value)}
          >
            {targets.map((target) => (
              <option key={target} value={target}>
                {target}
              </option>
            ))}
          </select>
        </label>
        <label className="grid gap-2 text-sm font-medium text-slate-700">
          Intent
          <select
            className="rounded-xl border border-slate-200 px-3 py-2 text-sm"
            value={formState.intent}
            onChange={(event) => handleChange("intent", event.target.value)}
          >
            {INTENTS.map((intent) => (
              <option key={intent} value={intent}>
                {intent}
              </option>
            ))}
          </select>
        </label>
        <label className="grid gap-2 text-sm font-medium text-slate-700 lg:col-span-2">
          Requested by
          <input
            className="rounded-xl border border-slate-200 px-3 py-2 text-sm"
            value={formState.requestedBy}
            onChange={(event) => handleChange("requestedBy", event.target.value)}
            placeholder="alex@contoso.com"
            required
          />
        </label>
      </div>
      <div className="flex flex-wrap items-center gap-3">
        <button
          type="submit"
          disabled={status === "saving"}
          className="rounded-xl bg-slate-900 px-4 py-2 text-sm font-semibold text-white shadow hover:bg-slate-800 disabled:cursor-not-allowed disabled:bg-slate-400"
        >
          {status === "saving" ? "Submitting..." : "Create publish request"}
        </button>
        {status === "success" ? (
          <span className="text-sm font-medium text-emerald-600">
            Request created.
          </span>
        ) : null}
        {status === "error" ? (
          <span className="text-sm font-medium text-rose-600">
            Something went wrong.
          </span>
        ) : null}
      </div>
    </form>
  );
}
