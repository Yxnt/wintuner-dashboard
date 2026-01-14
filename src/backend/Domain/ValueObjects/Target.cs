using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.ValueObjects;

public sealed record Target(TargetType Type, string Id);
