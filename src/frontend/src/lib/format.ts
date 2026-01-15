export function formatDateTime(value: string): string {
  return new Date(value).toLocaleString();
}

export function formatDate(value: string): string {
  return new Date(value).toLocaleDateString();
}
