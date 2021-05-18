using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MeowStory.Application.Common.Mappings;
using MeowStory.Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.Queries
{
    public class GetDraftStoriesWithPaginationQuery : IRequest<PaginatedList<StoryItemDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetDraftStoriesWithPaginationQueryHandler : IRequestHandler<GetDraftStoriesWithPaginationQuery, PaginatedList<StoryItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetDraftStoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<StoryItemDto>> Handle(GetDraftStoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var authorId = this._currentUserService.UserId;

            return await _context.Stories
                .Where(s => s.Author.Id == authorId && !s.DateReviewed.HasValue)
                .OrderByDescending(s => s.DateModified)
                .ProjectTo<StoryItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize); ;
        }
    }
}
