namespace NoruBanner.WebApi.Features.Statistics.Models;

public sealed record BannerStatisticsResponse(
    Guid BannerId,
    int TotalViews,
    int TotalClicks,
    int UniqueUsers,
    double ClickThroughRate);