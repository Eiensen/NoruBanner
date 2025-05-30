namespace NoruBanner.WebApi.Shared.Models;

public class BannerEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid BannerId { get; init; }
    public required string UserSessionId { get; init; }
    public required EventType EventType { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public string? AdditionalData { get; init; }
}
