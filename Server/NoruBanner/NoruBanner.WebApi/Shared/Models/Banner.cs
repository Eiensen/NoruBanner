namespace NoruBanner.WebApi.Shared.Models;

public class Banner
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public Guid SiteId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
