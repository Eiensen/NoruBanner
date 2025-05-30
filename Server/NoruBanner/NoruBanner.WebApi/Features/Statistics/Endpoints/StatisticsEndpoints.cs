using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoruBanner.WebApi.Features.Statistics.Models;
using NoruBanner.WebApi.Features.Statistics.Queries;

namespace NoruBanner.WebApi.Features.Statistics.Endpoints;

public static class StatisticsEndpoints
{
    public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/statistics")
            .WithTags("Statistics")
            .WithOpenApi();

        group.MapGet("/banner/{bannerId}", GetBannerStatistics)
            .WithName("GetBannerStatistics")
            .Produces<BannerStatisticsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

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