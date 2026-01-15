using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class JobEndpoints
{
    public static IEndpointRouteBuilder MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/jobs", (InMemoryDataStore store) => Results.Ok(store.GetJobs()));
        return app;
    }
}
