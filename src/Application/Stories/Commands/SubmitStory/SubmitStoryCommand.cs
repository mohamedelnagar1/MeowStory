using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Domain.Enums;

namespace MeowStory.Application.Stories.Commands.SubmitStory
{
    public class SubmitStoryCommand : IRequest
    {
        public string Id { get; set; }

    }

    public class SubmitStoryCommandHandler : IRequestHandler<SubmitStoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _datetime;
        private readonly ICurrentUserService _currentUserService;
        public SubmitStoryCommandHandler(IApplicationDbContext context, IDateTime dateTime,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _datetime = dateTime;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(SubmitStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Stories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Story), request.Id);
            }

            // Story could be only Submited by its writer
            if (entity.AuthorId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            entity.Submit(_datetime.Now);
            entity.DomainEvents.Add(new StorySubmittedEvent(entity)); // to find reviewer then assign and notify

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}