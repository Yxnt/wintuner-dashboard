using WintunerDashboard.WebApi.Endpoints;
using WintunerDashboard.WebApi.Services;

var app = builder.Build();

app.MapHealthEndpoints();
app.MapDashboardEndpoints();
app.MapPackageEndpoints();
app.MapPublishRequestEndpoints();
app.MapJobEndpoints();
app.MapSettingsEndpoints();
app.MapUpdateEndpoints();
