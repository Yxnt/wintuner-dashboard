using FluentAssertions;
using WintunerDashboard.Application.Services;
using WintunerDashboard.Domain.Enums;
using WintunerDashboard.Domain.ValueObjects;

namespace WintunerDashboard.Tests;

public class IdempotencyTests
{
    [Fact]
    public void TargetsNormalizer_SortsAndDedupes()
    {
        var targets = new[]
        {
            new Target(TargetType.Group, "b"),
            new Target(TargetType.User, "a"),
            new Target(TargetType.Group, "b"),
            new Target(TargetType.Group, "A")
        };

        var normalized = TargetsNormalizer.Normalize(targets);

        normalized.Should().Be("[{\"type\":0,\"id\":\"b\"},{\"type\":1,\"id\":\"a\"}]");
    }

    [Fact]
    public void IdempotencyKey_IsStable()
    {
        var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var targets = new[]
        {
            new Target(TargetType.Group, "g1"),
            new Target(TargetType.User, "u1")
        };
        var normalized = TargetsNormalizer.Normalize(targets);

        var key1 = IdempotencyKeyBuilder.Build(tenantId, "Contoso.App", "1.2.3", InstallIntent.Available, normalized);
        var key2 = IdempotencyKeyBuilder.Build(tenantId, "Contoso.App", "1.2.3", InstallIntent.Available, normalized);

        key1.Should().Be(key2);
    }
}
