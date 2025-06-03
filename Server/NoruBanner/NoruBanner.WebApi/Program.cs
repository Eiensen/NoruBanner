using NoruBanner.WebApi.Extensions;
using NoruBanner.WebApi.Features.Statistics.Endpoints;
using NoruBanner.WebApi.Features.Tracking.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Only use HTTPS redirection if not in Docker
if (!builder.Environment.IsEnvironment("Docker"))
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 443;
    });
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before other middleware
app.UseCors();

// Only use HTTPS redirection if not in Docker
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.MapTrackingEndpoints();
app.MapStatisticsEndpoints();

// Add health check endpoint
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();