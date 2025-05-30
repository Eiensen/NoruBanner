using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Shared.Interfaces;

public interface IBannerStatisticsRepository
{
    Task<BannerStatistics> GetBannerStatisticsAsync(Guid bannerId);
    Task<Dictionary<Guid, BannerStatistics>> GetBannersStatisticsAsync(IEnumerable<Guid> bannerIds);
}

