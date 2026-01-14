using System.Security.Cryptography;
using System.Text;
using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Application.Services;

public static class IdempotencyKeyBuilder
{
    public static string Build(Guid tenantId, string packageIdentifier, string version, InstallIntent intent, string targetsNormalized)
    {
        var payload = string.Concat(tenantId.ToString("N"), packageIdentifier, version, intent.ToString(), targetsNormalized);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
