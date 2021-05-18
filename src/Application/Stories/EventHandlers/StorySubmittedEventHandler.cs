using MeowStory.Application.Common.Models;
using MeowStory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MeowStory.Application.Stories.EventHandlers
{
    public class StorySubmittedEventHandler : INotificationHandler<DomainEventNotification<StorySubmittedEvent>>
    {
        private readonly ILogger<StorySubmittedEventHandler> _logger;
        private readonly IApplicationDbContext _context;
        public StorySubmittedEventHandler(ILogger<StorySubmittedEventHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Handle(DomainEventNotification<StorySubmittedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("MeowStory Domain Event: {DomainEvent}", domainEvent.GetType().Name);


            // Find reviewer who has less assigned stories

            //var reviewer = await (from a in _context.Reviewers
            //                      join s in _context.Stories.Where(s=> !s.DateReviewed.HasValue)
            //                      on a.Id equals s.ReviewerId into j
            //                      from x in j.DefaultIfEmpty()
            //                      group a by a.Id into g
            //                      orderby g.Count()
            //                      select g.FirstOrDefault()).FirstOrDefaultAsync();

            var reviewer = await (from a in _context.Reviewers
                                  orderby a.StoriesReviewed.Count() descending
                                  select a).FirstOrDefaultAsync();

            var story = await _context.Stories.FindAsync(notification.DomainEvent.Story.Id);

            story.AssignReviewer(reviewer.Id);


            await _context.SaveChangesAsync(cancellationToken);

            //TODO: Send Email to reviewer
        }
    }
}
