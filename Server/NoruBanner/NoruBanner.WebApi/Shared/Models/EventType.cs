namespace NoruBanner.WebApi.Shared.Models;

/// <summary>
/// Types of banner interaction events
/// </summary>
public enum EventType
{
    /// <summary>
    /// The banner was viewed by a user (at least 50% visible in viewport)
    /// </summary>
    Viewed = 1,

    /// <summary>
    /// The banner was clicked by a user
    /// </summary>
    Clicked = 2
}
