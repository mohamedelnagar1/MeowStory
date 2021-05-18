using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Domain.Enums;

namespace MeowStory.Application.Stories.Commands.DeleteStory
{
    public class DeleteStoryCommand : IRequest
    {
        public string Id { get; set; }

    }

    public class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteStoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Stories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Story), request.Id);
            }

            _context.Stories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}