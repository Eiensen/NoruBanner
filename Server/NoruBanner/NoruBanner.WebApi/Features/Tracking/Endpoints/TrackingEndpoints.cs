using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoruBanner.WebApi.Features.Tracking.Commands;
using Microsoft.AspNetCore.Http;

namespace NoruBanner.WebApi.Features.Tracking.Endpoints;

public static class TrackingEndpoints
{    
    public static void MapTrackingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tracking")
            .WithTags("Tracking")
            .WithOpenApi();

        group.MapPost("/event", RecordEvent)
            .WithName("RecordBannerEvent")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> RecordEvent(
        [FromBody] RecordEventCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext.Request.Headers.UserAgent.ToString();

        var result = await sender.Send(
            command with { IpAddress = ipAddress, UserAgent = userAgent },
            cancellationToken);

        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
}