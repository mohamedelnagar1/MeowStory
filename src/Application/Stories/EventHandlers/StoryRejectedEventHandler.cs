using MeowStory.Application.Common.Models;
using MeowStory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.EventHandlers
{
    public class StoryRejectedEventHandler : INotificationHandler<DomainEventNotification<StoryRejectedEvent>>
    {
        private readonly ILogger<StoryRejectedEventHandler> _logger;

        public StoryRejectedEventHandler(ILogger<StoryRejectedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<StoryRejectedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("MeowStory Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            //TODO: Send Email to author 

            return Task.CompletedTask;
        }
    }
}
