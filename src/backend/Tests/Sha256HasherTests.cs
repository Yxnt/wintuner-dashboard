using FluentAssertions;
using WintunerDashboard.Application.Services;

namespace WintunerDashboard.Tests;

public class Sha256HasherTests
{
    [Fact]
    public void ComputesExpectedHash()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "hello");
            var hash = Sha256Hasher.ComputeHex(tempFile);
            hash.Should().Be("2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
