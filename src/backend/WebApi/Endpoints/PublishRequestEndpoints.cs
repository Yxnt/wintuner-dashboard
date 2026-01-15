using WintunerDashboard.WebApi.Models;
using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class PublishRequestEndpoints
{
    public static IEndpointRouteBuilder MapPublishRequestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/publish-requests", (InMemoryDataStore store) =>
            Results.Ok(store.GetPublishRequests()));

        app.MapPost("/api/publish-requests", (CreatePublishRequestRequest request, InMemoryDataStore store) =>
        {
            var created = store.CreatePublishRequest(request);
            return Results.Created($"/api/publish-requests/{created.Id}", created);
        });

        return app;
    }
}
