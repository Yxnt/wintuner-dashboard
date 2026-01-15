using WintunerDashboard.WebApi.Endpoints;
using WintunerDashboard.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<InMemoryDataStore>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthEndpoints();
app.MapDashboardEndpoints();
app.MapPackageEndpoints();
app.MapPublishRequestEndpoints();
app.MapJobEndpoints();
app.MapSettingsEndpoints();
app.MapUpdateEndpoints();

app.Run();
