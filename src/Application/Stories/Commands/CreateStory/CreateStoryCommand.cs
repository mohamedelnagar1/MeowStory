using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.Commands.CreateStory
{
    public class CreateStoryCommand : IRequest<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _datetime;
        private readonly ICurrentUserService _currentUserService;

        public CreateStoryCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
        {
            _context = context;
            _datetime = dateTime;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Story(request.Title, request.Description, _currentUserService.UserId);

            _context.Stories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}