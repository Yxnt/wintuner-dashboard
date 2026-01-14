using FluentAssertions;
using WintunerDashboard.Domain.Enums;
using WintunerDashboard.Domain.Services;

namespace WintunerDashboard.Tests;

public class PublishRequestStateMachineTests
{
    [Fact]
    public void AllowsValidTransitions()
    {
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.Draft, PublishRequestStatus.PendingApproval).Should().BeTrue();
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.PendingApproval, PublishRequestStatus.Approved).Should().BeTrue();
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.Running, PublishRequestStatus.Succeeded).Should().BeTrue();
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.Failed, PublishRequestStatus.Running).Should().BeTrue();
    }

    [Fact]
    public void RejectsInvalidTransitions()
    {
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.Rejected, PublishRequestStatus.Running).Should().BeFalse();
        PublishRequestStateMachine.CanTransition(PublishRequestStatus.Succeeded, PublishRequestStatus.Running).Should().BeFalse();
    }
}
