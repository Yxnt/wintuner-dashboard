using System.Text.Json;
using WintunerDashboard.Domain.ValueObjects;

namespace WintunerDashboard.Application.Services;

public static class TargetsNormalizer
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static string Normalize(IEnumerable<Target> targets)
    {
        var normalized = targets
            .Select(target => new Target(target.Type, target.Id.Trim()))
            .DistinctBy(
                target => (target.Type.ToUpperInvariant(), target.Id.ToUpperInvariant()),
                EqualityComparer<(string Type, string Id)>.Default)
            .OrderBy(target => target.Type, StringComparer.OrdinalIgnoreCase)
            .ThenBy(target => target.Id, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return JsonSerializer.Serialize(normalized, SerializerOptions);
    }
}
