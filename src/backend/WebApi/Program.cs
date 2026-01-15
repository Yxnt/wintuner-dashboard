using Microsoft.EntityFrameworkCore;
using WintunerDashboard.Infrastructure.Persistence;
using WintunerDashboard.WebApi;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/health", () => Results.Ok("ok"));
app.MapApiEndpoints();

app.Run();
