using Microsoft.EntityFrameworkCore;
using NoruBanner.WebApi.Shared.Interfaces;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Infrastructure.Persistence.Repositories;

public class BannerEventRepository (AppDbContext context) : IBannerEventRepository
{
    public async Task<BannerEvent?> GetByIdAsync(Guid id)
        => await context.BannerEvents.FindAsync(id);

    public async Task<IReadOnlyList<BannerEvent>> GetAllAsync()
        => await context.BannerEvents.AsNoTracking().ToListAsync();

    public async Task<Guid> AddAsync(BannerEvent entity)
    {
        context.BannerEvents.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(BannerEvent entity)
    {
        context.BannerEvents.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BannerEvent entity)
    {
        context.BannerEvents.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetViewCountAsync(Guid bannerId)
        => await context.BannerEvents
            .CountAsync(e => e.BannerId == bannerId && e.EventType == EventType.Viewed);

    public async Task<int> GetClickCountAsync(Guid bannerId)
        => await context.BannerEvents
            .CountAsync(e => e.BannerId == bannerId && e.EventType == EventType.Clicked);

    public async Task<int> GetUniqueUserCountAsync(Guid bannerId)
        => await context.BannerEvents
            .Where(e => e.BannerId == bannerId)
            .Select(e => e.UserSessionId)
            .Distinct()
            .CountAsync();

    public async Task AddRangeAsync(IEnumerable<BannerEvent> entities)
    {
        await context.BannerEvents.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}
