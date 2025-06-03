using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoruBanner.WebApi.Features.Statistics.Models;
using NoruBanner.WebApi.Features.Statistics.Queries;
using Microsoft.AspNetCore.Http;

namespace NoruBanner.WebApi.Features.Statistics.Endpoints;

/// <summary>
/// Endpoints for retrieving banner statistics
/// </summary>
public static class StatisticsEndpoints
{
    /// <summary>
    /// Maps all statistics related endpoints
    /// </summary>
    /// <param name="app">The endpoint route builder</param>
    public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/statistics")
            .WithTags("Statistics")
            .WithOpenApi();

        group.MapGet("/banner/{bannerId}", GetBannerStatistics)
            .WithName("GetBannerStatistics")
            .Produces<BannerStatisticsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("Gets statistics for a specific banner including views, clicks, and unique visitors")
            .WithSummary("Get banner statistics");
    }

    /// <summary>
    /// Retrieves statistics for a specific banner
    /// </summary>
    /// <param name="bannerId">The unique identifier of the banner</param>
    /// <param name="sender">MediatR sender for dispatching queries</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>200 OK with statistics if found, 404 Not Found if banner doesn't exist</returns>
    private static async Task<IResult> GetBannerStatistics(
        Guid bannerId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetBannerStatisticsQuery(bannerId);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }
}