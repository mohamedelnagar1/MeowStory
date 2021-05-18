using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MeowStory.Application.Common.Mappings;
using MeowStory.Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MeowStory.Application.Stories.Queries
{
    public class GetStoryCommentsWithPaginationQuery : IRequest<PaginatedList<CommentDto>>
    {
        public string StoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetStoryCommentsWithPaginationQueryHandler : IRequestHandler<GetStoryCommentsWithPaginationQuery, PaginatedList<CommentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStoryCommentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CommentDto>> Handle(GetStoryCommentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Comments
                .Include(c=> c.User)
                .Where(c => c.StoryId == request.StoryId)
                .OrderByDescending(s => s.DateCreated)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
