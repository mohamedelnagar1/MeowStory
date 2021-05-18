using MeowStory.Application.Common.Models;
using MeowStory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.EventHandlers
{
    public class CommentAddedEventHandler : INotificationHandler<DomainEventNotification<CommentAddedEvent>>
    {
        private readonly ILogger<CommentAddedEventHandler> _logger;

        public CommentAddedEventHandler(ILogger<CommentAddedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<CommentAddedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("MeowStory Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            // Send Email To Author

            return Task.CompletedTask;
        }
    }
}
