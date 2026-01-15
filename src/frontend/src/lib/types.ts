export type DashboardStats = {
  totalPackages: number;
  pendingPublishRequests: number;
  runningJobs: number;
  failedJobs: number;
  publishedApps: number;
};

export type PublishRequestItem = {
  id: string;
  packageId: string;
  packageName: string;
  version: string;
  target: string;
  intent: string;
  status: string;
  requestedAt: string;
  requestedBy: string;
};

export type JobItem = {
  id: string;
  packageId: string;
  packageName: string;
  version: string;
  step: string;
  status: string;
  progress: number;
  startedAt: string;
  completedAt?: string | null;
};

export type DashboardSummary = {
  stats: DashboardStats;
  recentPublishRequests: PublishRequestItem[];
  recentJobs: JobItem[];
};

export type PackageVersionItem = {
  version: string;
  releaseChannel: string;
  publishedAt: string;
  sha256: string;
};

export type PackageItem = {
  id: string;
  name: string;
  publisher: string;
  currentVersion: string;
  status: string;
  lastUpdated: string;
  versions: PackageVersionItem[];
};

export type UpdateEventItem = {
  id: string;
  packageId: string;
  packageName: string;
  currentVersion: string;
  latestVersion: string;
  status: string;
  severity: string;
  detectedAt: string;
  actionOwner: string;
};

export type SettingsResponse = {
  tenantName: string;
  notificationMailbox: string;
  defaultTarget: string;
  environment: string;
  availableGroups: string[];
};

export type CreatePublishRequest = {
  packageId: string;
  version: string;
  target: string;
  intent: string;
  requestedBy: string;
};
