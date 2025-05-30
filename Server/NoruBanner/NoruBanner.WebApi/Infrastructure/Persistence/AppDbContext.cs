using Microsoft.EntityFrameworkCore;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Infrastructure.Persistence;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<BannerEvent> BannerEvents { get; set; }
    public DbSet<Banner> Banners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BannerEvent>()
            .HasIndex(e => e.UserSessionId);

        modelBuilder.Entity<BannerEvent>()
            .HasIndex(e => e.BannerId);

        modelBuilder.Entity<Banner>()
            .HasIndex(b => b.SiteId);
    }
}
