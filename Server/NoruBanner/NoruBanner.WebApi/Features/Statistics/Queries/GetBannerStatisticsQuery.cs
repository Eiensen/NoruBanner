using MediatR;
using NoruBanner.WebApi.Features.Statistics.Models;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Statistics.Queries;

public sealed record GetBannerStatisticsQuery(Guid BannerId)
    : IRequest<Result<BannerStatisticsResponse>>;