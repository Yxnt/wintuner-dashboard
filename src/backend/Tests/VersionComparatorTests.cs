using FluentAssertions;
using WintunerDashboard.Domain.Services;

namespace WintunerDashboard.Tests;

public class VersionComparatorTests
{
    [Theory]
    [InlineData("1.2", "1.2.0", true)]
    [InlineData("1.2.10", "1.2.9", true)]
    [InlineData("1.2.3-beta", "1.2.3", false)]
    [InlineData("2024.01", "2023.12", true)]
    [InlineData("1.0-alpha", "1.0-beta", false)]
    [InlineData("2.0", "10.0", false)]
    public void IsAtLeast_ComparesVersions(string installed, string target, bool expected)
    {
        VersionComparator.IsAtLeast(installed, target).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "1.0")]
    [InlineData("", "1.0")]
    [InlineData("1.0", null)]
    public void IsAtLeast_ReturnsFalseOnInvalidInputs(string? installed, string? target)
    {
        VersionComparator.IsAtLeast(installed, target).Should().BeFalse();
    }
}
