using MediatR;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Tracking.Commands;

public sealed record RecordEventCommand(
    Guid BannerId,
    string UserSessionId,
    EventType EventType,
    string? IpAddress = null,
    string? UserAgent = null) : IRequest<Result>;