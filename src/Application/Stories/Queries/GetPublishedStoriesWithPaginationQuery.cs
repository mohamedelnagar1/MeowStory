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
    public class GetPublishedStoriesWithPaginationQuery : IRequest<PaginatedList<StoryItemDto>>
    {
        public string AuthorId { get; set; }
        public string GenreId { get; set; }
        public string SearchParam { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetStoriesWithPaginationQueryHandler : IRequestHandler<GetPublishedStoriesWithPaginationQuery, PaginatedList<StoryItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<StoryItemDto>> Handle(GetPublishedStoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Stories
                .Where(s => s.DateReviewed.HasValue);

            if (!string.IsNullOrEmpty(request.AuthorId))
                query = query.Where(s => s.AuthorId == request.AuthorId);

            if (!string.IsNullOrEmpty(request.GenreId))
                query = query.Where(s => s.GenreId == request.GenreId);

            if (!string.IsNullOrEmpty(request.SearchParam))
                query = query.Where(s => s.Title.Contains(request.SearchParam));


            return await query.OrderByDescending(s => s.DateReviewed)
                .ThenBy(s => s.Views)
                .ProjectTo<StoryItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize); ;
        }
    }
}
