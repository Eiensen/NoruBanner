using MediatR;
using NoruBanner.WebApi.Features.Statistics.Models;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Statistics.Queries;

/// <summary>
/// Query to retrieve statistics for a specific banner
/// </summary>
/// <param name="BannerId">Unique identifier of the banner to get statistics for</param>
public sealed record GetBannerStatisticsQuery(Guid BannerId)
    : IRequest<Result<BannerStatisticsResponse>>;