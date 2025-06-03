using Microsoft.EntityFrameworkCore;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ILogger<AppDbContext> logger)
        : base(options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public DbSet<BannerEvent> BannerEvents => Set<BannerEvent>();
    public DbSet<Banner> Banners => Set<Banner>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<BannerEvent>(entity =>
        {
            entity.ToTable("BannerEvents");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserSessionId)
                .HasDatabaseName("IX_BannerEvents_UserSessionId");

            entity.HasIndex(e => e.BannerId)
                .HasDatabaseName("IX_BannerEvents_BannerId");

            entity.Property(e => e.UserSessionId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.AdditionalData)
                .HasMaxLength(1000);

            entity.Property(e => e.Timestamp)
                .IsRequired()
                .HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.ToTable("Banners");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.SiteId)
                .HasDatabaseName("IX_Banners_SiteId");

            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes to database at {Time}", DateTime.UtcNow);
            throw;
        }
    }
}
