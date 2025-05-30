using MediatR;
using NoruBanner.WebApi.Features.Statistics.Models;
using NoruBanner.WebApi.Features.Statistics.Queries;
using NoruBanner.WebApi.Shared.Interfaces;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Statistics.Handlers;

internal sealed class GetBannerStatisticsQueryHandler(
    IBannerEventRepository repository)
    : IRequestHandler<GetBannerStatisticsQuery, Result<BannerStatisticsResponse>>
{
    public async Task<Result<BannerStatisticsResponse>> Handle(
        GetBannerStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var views = await repository.GetViewCountAsync(request.BannerId);
        var clicks = await repository.GetClickCountAsync(request.BannerId);
        var uniqueUsers = await repository.GetUniqueUserCountAsync(request.BannerId);

        var response = new BannerStatisticsResponse(
            request.BannerId,
            views,
            clicks,
            uniqueUsers,
            views > 0 ? (double)clicks / views : 0);

        return Result.Success(response);
    }
}