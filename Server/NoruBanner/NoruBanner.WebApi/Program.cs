using NoruBanner.WebApi.Extensions;
using NoruBanner.WebApi.Features.Statistics.Endpoints;
using NoruBanner.WebApi.Features.Tracking.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTrackingEndpoints();
app.MapStatisticsEndpoints();

app.Run();