using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Infrastructure.Persistence;
using WintunerDashboard.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WintunerDbContext>(options =>
{
    var configuration = builder.Configuration;
    var host = configuration["POSTGRES_HOST"] ?? "localhost";
    var port = configuration["POSTGRES_PORT"] ?? "5432";
    var database = configuration["POSTGRES_DB"] ?? "wintuner";
    var user = configuration["POSTGRES_USER"] ?? "wintuner";
    var password = configuration["POSTGRES_PASSWORD"] ?? "change_me";
    var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok("ok"));
app.MapApiEndpoints();

app.MapGet("/api/dashboard", (InMemoryDataStore store) =>
{
    var summary = store.GetDashboardSummary();
    return Results.Ok(summary);
});

app.MapGet("/api/packages", (InMemoryDataStore store) => Results.Ok(store.GetPackages()));

app.MapGet("/api/packages/{packageId}", (string packageId, InMemoryDataStore store) =>
{
    var package = store.GetPackage(packageId);
    return package is null ? Results.NotFound() : Results.Ok(package);
});

app.MapGet("/api/publish-requests", (InMemoryDataStore store) => Results.Ok(store.GetPublishRequests()));

app.MapPost("/api/publish-requests", (CreatePublishRequestRequest request, InMemoryDataStore store) =>
{
    var created = store.CreatePublishRequest(request);
    return Results.Created($"/api/publish-requests/{created.Id}", created);
});

app.MapGet("/api/jobs", (InMemoryDataStore store) => Results.Ok(store.GetJobs()));

app.MapGet("/api/settings", (InMemoryDataStore store) => Results.Ok(store.GetSettings()));

app.Run();
