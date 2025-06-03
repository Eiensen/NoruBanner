using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoruBanner.WebApi.Features.Tracking.Commands;

namespace NoruBanner.WebApi.Features.Tracking.Endpoints;

/// <summary>
/// Endpoints for tracking banner events like views and clicks
/// </summary>
public static class TrackingEndpoints
{
    /// <summary>
    /// Maps all tracking related endpoints
    /// </summary>
    /// <param name="app">The endpoint route builder</param>
    public static void MapTrackingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tracking")
            .WithTags("Tracking")
            .WithOpenApi();

        group.MapPost("/event", RecordEvent)
            .WithName("RecordBannerEvent")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Records a banner event (view or click)")
            .WithSummary("Record banner interaction event");
    }

    /// <summary>
    /// Records a banner interaction event (view or click)
    /// </summary>
    /// <param name="command">The event details including banner ID, session ID, and event type</param>
    /// <param name="sender">MediatR sender for dispatching commands</param>
    /// <param name="httpContext">HTTP context for accessing client information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>200 OK if successful, 400 Bad Request if validation fails</returns>
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