namespace NoruBanner.WebApi.Shared.Models;

public class BannerStatistics
{
    public Guid BannerId { get; set; }
    public int TotalViews { get; set; }
    public int TotalClicks { get; set; }
    public int UniqueUsers { get; set; }
    public double ClickThroughRate { get; set; }
}
