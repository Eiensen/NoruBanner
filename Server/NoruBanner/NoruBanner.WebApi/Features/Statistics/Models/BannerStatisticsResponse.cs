namespace NoruBanner.WebApi.Features.Statistics.Models;

/// <summary>
/// Response model containing banner statistics
/// </summary>
/// <param name="BannerId">Unique identifier of the banner</param>
/// <param name="TotalViews">Total number of times the banner was viewed</param>
/// <param name="TotalClicks">Total number of times the banner was clicked</param>
/// <param name="UniqueUsers">Number of unique users who interacted with the banner</param>
/// <param name="ClickThroughRate">Click-through rate (CTR) calculated as clicks/views</param>
public sealed record BannerStatisticsResponse(
    Guid BannerId,
    int TotalViews,
    int TotalClicks,
    int UniqueUsers,
    double ClickThroughRate);