using MediatR;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Tracking.Commands;

/// <summary>
/// Command for recording a banner interaction event
/// </summary>
/// <param name="BannerId">Unique identifier of the banner that was interacted with</param>
/// <param name="UserSessionId">Unique identifier of the user session</param>
/// <param name="EventType">Type of the event (Viewed or Clicked)</param>
/// <param name="IpAddress">IP address of the user (automatically populated)</param>
/// <param name="UserAgent">User agent string of the browser (automatically populated)</param>
/// <example>
/// {
///   "bannerId": "123e4567-e89b-4456-8af1-123e4567abcd",
///   "userSessionId": "session-98765432-1234-5678-9012-345678901234",
///   "eventType": "Viewed"
/// }
/// </example>
public sealed record RecordEventCommand(
    Guid BannerId,
    string UserSessionId,
    EventType EventType,
    string? IpAddress = null,
    string? UserAgent = null) : IRequest<Result>;