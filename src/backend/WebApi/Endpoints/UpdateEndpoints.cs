using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class UpdateEndpoints
{
    public static IEndpointRouteBuilder MapUpdateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/updates", (InMemoryDataStore store) => Results.Ok(store.GetUpdates()));
        return app;
    }
}
