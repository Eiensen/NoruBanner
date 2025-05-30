using MediatR;
using NoruBanner.WebApi.Features.Tracking.Commands;
using NoruBanner.WebApi.Shared.Interfaces;
using NoruBanner.WebApi.Shared.Models;

namespace NoruBanner.WebApi.Features.Tracking.Handlers;

internal sealed class RecordEventCommandHandler(
    IBannerEventRepository repository,
    ILogger<RecordEventCommandHandler> logger)
    : IRequestHandler<RecordEventCommand, Result>
{
    public async Task<Result> Handle(
        RecordEventCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var bannerEvent = new BannerEvent
            {
                BannerId = request.BannerId,
                UserSessionId = request.UserSessionId,
                EventType = request.EventType,
                AdditionalData = $"UserAgent: {request.UserAgent}, IP: {request.IpAddress}"
            };

            await repository.AddAsync(bannerEvent);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error recording banner event");
            return Result.Failure("Error recording event");
        }
    }
}
