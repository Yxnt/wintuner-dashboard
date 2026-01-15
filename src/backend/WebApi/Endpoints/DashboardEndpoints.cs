using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class DashboardEndpoints
{
    public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dashboard", (InMemoryDataStore store) => Results.Ok(store.GetDashboardSummary()));
        return app;
    }
}
