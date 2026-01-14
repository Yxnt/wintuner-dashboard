using System.Security.Cryptography;

namespace WintunerDashboard.Application.Services;

public static class Sha256Hasher
{
    public static string ComputeHex(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
