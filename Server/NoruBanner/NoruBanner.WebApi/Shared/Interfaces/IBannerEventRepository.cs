using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Shared.Interfaces;

public interface IBannerEventRepository : IRepository<BannerEvent>
{
    Task<int> GetViewCountAsync(Guid bannerId);
    Task<int> GetClickCountAsync(Guid bannerId);
    Task<int> GetUniqueUserCountAsync(Guid bannerId);
    Task AddRangeAsync(IEnumerable<BannerEvent> entities);
}
