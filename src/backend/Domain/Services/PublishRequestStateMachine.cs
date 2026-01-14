using WintunerDashboard.Domain.Enums;

namespace WintunerDashboard.Domain.Services;

public static class PublishRequestStateMachine
{
    private static readonly Dictionary<PublishRequestStatus, PublishRequestStatus[]> AllowedTransitions = new()
    {
        { PublishRequestStatus.Draft, new[] { PublishRequestStatus.PendingApproval } },
        { PublishRequestStatus.PendingApproval, new[] { PublishRequestStatus.Approved, PublishRequestStatus.Rejected } },
        { PublishRequestStatus.Approved, new[] { PublishRequestStatus.Running } },
        { PublishRequestStatus.Running, new[] { PublishRequestStatus.Succeeded, PublishRequestStatus.Failed } },
        { PublishRequestStatus.Failed, new[] { PublishRequestStatus.Running } }
    };

    public static bool CanTransition(PublishRequestStatus current, PublishRequestStatus next)
        => AllowedTransitions.TryGetValue(current, out var nextStates) && nextStates.Contains(next);

    public static void EnsureTransition(PublishRequestStatus current, PublishRequestStatus next)
    {
        if (!CanTransition(current, next))
        {
            throw new InvalidOperationException($"Invalid transition: {current} -> {next}.");
        }
    }
}
