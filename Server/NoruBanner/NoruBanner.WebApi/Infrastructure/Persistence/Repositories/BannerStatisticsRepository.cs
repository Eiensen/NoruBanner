using Microsoft.EntityFrameworkCore;
using NoruBanner.WebApi.Shared.Interfaces;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Infrastructure.Persistence.Repositories;

public class BannerStatisticsRepository(AppDbContext context) : IBannerStatisticsRepository
{
    public async Task<BannerStatistics> GetBannerStatisticsAsync(Guid bannerId)
    {
        var stats = new BannerStatistics { BannerId = bannerId };

        var eventsQuery = context.BannerEvents
            .Where(e => e.BannerId == bannerId);

        var baseStats = await eventsQuery
            .GroupBy(e => 1)
            .Select(g => new
            {
                TotalViews = g.Count(e => e.EventType == EventType.Viewed),
                TotalClicks = g.Count(e => e.EventType == EventType.Clicked),
                UniqueUsers = g.Select(e => e.UserSessionId).Distinct().Count()
            })
            .FirstOrDefaultAsync();

        if (baseStats != null)
        {
            stats.TotalViews = baseStats.TotalViews;
            stats.TotalClicks = baseStats.TotalClicks;
            stats.UniqueUsers = baseStats.UniqueUsers;
            stats.ClickThroughRate = baseStats.TotalViews > 0
                ? (double)baseStats.TotalClicks / baseStats.TotalViews
                : 0;
        }

        return stats;
    }

    public async Task<Dictionary<Guid, BannerStatistics>> GetBannersStatisticsAsync(IEnumerable<Guid> bannerIds)
    {
        var ids = bannerIds.ToList();
        var result = new Dictionary<Guid, BannerStatistics>();

        if (!ids.Any())
            return result;

        var baseStats = await context.BannerEvents
            .Where(e => ids.Contains(e.BannerId))
            .GroupBy(e => e.BannerId)
            .Select(g => new
            {
                BannerId = g.Key,
                TotalViews = g.Count(e => e.EventType == EventType.Viewed),
                TotalClicks = g.Count(e => e.EventType == EventType.Clicked)
            })
            .ToListAsync();

        var uniqueUsersStats = await context.BannerEvents
            .Where(e => ids.Contains(e.BannerId))
            .GroupBy(e => e.BannerId)
            .Select(g => new
            {
                BannerId = g.Key,
                UniqueUsers = g.Select(e => e.UserSessionId).Distinct().Count()
            })
            .ToListAsync();

        foreach (var id in ids)
        {
            var baseStat = baseStats.FirstOrDefault(s => s.BannerId == id);
            var uniqueStat = uniqueUsersStats.FirstOrDefault(s => s.BannerId == id);

            var stat = new BannerStatistics
            {
                BannerId = id,
                TotalViews = baseStat?.TotalViews ?? 0,
                TotalClicks = baseStat?.TotalClicks ?? 0,
                UniqueUsers = uniqueStat?.UniqueUsers ?? 0,
                ClickThroughRate = baseStat?.TotalViews > 0
                    ? (double)(baseStat.TotalClicks) / baseStat.TotalViews
                    : 0
            };

            result.Add(id, stat);
        }

        return result;
    }
}
