using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Domain.Enums;

namespace MeowStory.Application.Stories.Commands.ReviewStory
{
    public class ReviewStoryCommand : IRequest
    {
        public string Id { get; set; }
        public bool IsApproved { get; set; }
        public string RejectionReason { get; set; }
    }

    public class ReviewStoryCommandHandler : IRequestHandler<ReviewStoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _datetime;
        private readonly ICurrentUserService _currentUserService;

        public ReviewStoryCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
        {
            _context = context;
            _datetime = dateTime;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ReviewStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Stories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Story), request.Id);
            }

            // Story could be only reviewed by its assigned reviewer
            if (entity.ReviewerId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            if (request.IsApproved)
            {
                entity.Approve(_datetime.Now);
                entity.DomainEvents.Add(new StoryPublishedEvent(entity));
            }
            else
            {
                entity.Reject(_datetime.Now, request.RejectionReason);
                entity.DomainEvents.Add(new StoryRejectedEvent(entity));
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}