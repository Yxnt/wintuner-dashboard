using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class SettingsEndpoints
{
    public static IEndpointRouteBuilder MapSettingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/settings", (InMemoryDataStore store) => Results.Ok(store.GetSettings()));
        return app;
    }
}
