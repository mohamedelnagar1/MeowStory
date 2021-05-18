using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;

namespace MeowStory.Application.Stories.Commands.VoteStory
{
    public class VoteStoryCommand : IRequest
    {
        public string Id { get; set; }

    }

    public class VoteStoryCommandHandler : IRequestHandler<VoteStoryCommand>
    {
        private readonly IApplicationDbContext _context;
        public VoteStoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(VoteStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Stories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Story), request.Id);
            }

            entity.IncrementVotes();

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}