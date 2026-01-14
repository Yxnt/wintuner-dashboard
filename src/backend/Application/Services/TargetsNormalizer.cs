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
            .DistinctBy(target => ($"{target.Type}", target.Id), StringComparer.OrdinalIgnoreCase)
            .OrderBy(target => target.Type)
            .ThenBy(target => target.Id, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return JsonSerializer.Serialize(normalized, SerializerOptions);
    }
}
