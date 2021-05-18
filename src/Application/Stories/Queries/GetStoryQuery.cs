using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MeowStory.Application.Stories.Queries
{
    public class GetStoryQuery : IRequest<StoryDto>
    {
        public string Id { get; set; }
    }

    public class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, StoryDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StoryDto> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            return await _context.Stories
                .Where(s => s.Id == request.Id)
                .ProjectTo<StoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
