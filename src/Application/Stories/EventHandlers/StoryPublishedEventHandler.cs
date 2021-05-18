using MeowStory.Application.Common.Models;
using MeowStory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.EventHandlers
{
    public class StoryPublishedEventHandler : INotificationHandler<DomainEventNotification<StoryPublishedEvent>>
    {
        private readonly ILogger<StoryPublishedEventHandler> _logger;

        public StoryPublishedEventHandler(ILogger<StoryPublishedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<StoryPublishedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("MeowStory Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            //TODO: Send Recommendaion Email to Users

            return Task.CompletedTask;
        }
    }
}
