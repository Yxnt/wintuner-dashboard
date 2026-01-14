namespace WintunerDashboard.Domain.Enums;

public enum PublishJobStep
{
    Resolve = 0,
    Download = 1,
    VerifyHash = 2,
    Pack = 3,
    CreateOrUpdateApp = 4,
    Upload = 5,
    Assign = 6,
    Done = 7
}
