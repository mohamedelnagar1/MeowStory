using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeowStory.Application.Stories.Queries.Export
{
    public class ExportStoryQuery : IRequest<ExportStoryVm>
    {
        public string StoryId { get; set; }
    }

    public class ExportStoryQueryHandler : IRequestHandler<ExportStoryQuery, ExportStoryVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPdfFileBuilder _pdfBuilder;

        public ExportStoryQueryHandler(IApplicationDbContext context, IMapper mapper, IPdfFileBuilder pdfBuilder)
        {
            _context = context;
            _mapper = mapper;
            _pdfBuilder = pdfBuilder;
        }

        public async Task<ExportStoryVm> Handle(ExportStoryQuery request, CancellationToken cancellationToken)
        {
            var story =  await _context.Stories
                .Include(s => s.Author)
                .Where(s => s.Id == request.StoryId)
                .ProjectTo<StoryExportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var vm = new ExportStoryVm();
            vm.Content =  _pdfBuilder.BuildStoryDocument(story);

            vm.ContentType = "application/pdf";
            vm.FileName = $"{Guid.NewGuid()}.pdf";

            return await Task.FromResult(vm);

        }
    }
}
