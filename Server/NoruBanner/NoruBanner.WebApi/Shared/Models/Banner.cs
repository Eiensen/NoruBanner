namespace NoruBanner.WebApi.Shared.Models;

public class Banner
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string ImageUrl { get; set; }
    public Guid SiteId { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
