using WintunerDashboard.WebApi.Services;

namespace WintunerDashboard.WebApi.Endpoints;

public static class PackageEndpoints
{
    public static IEndpointRouteBuilder MapPackageEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/packages", (InMemoryDataStore store) => Results.Ok(store.GetPackages()));

        app.MapGet("/api/packages/{packageId}", (string packageId, InMemoryDataStore store) =>
        {
            var package = store.GetPackage(packageId);
            return package is null ? Results.NotFound() : Results.Ok(package);
        });

        return app;
    }
}
