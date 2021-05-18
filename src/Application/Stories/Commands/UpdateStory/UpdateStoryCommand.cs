using MeowStory.Application.Common.Interfaces;
using MeowStory.Domain.Entities;
using MeowStory.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;

namespace MeowStory.Application.Stories.Commands.UpdateStory
{
    public class UpdateStoryCommand : IRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GenreId { get; set; }
        public string Content { get; set; }
        public byte[] Photo { get; set; } = null; // TODO: Complete Uploader implementation
        public byte[] Thumbnail { get; set; } = null; // TODO: Complete Uploader implementation

    }

    public class UpdateStoryCommandHandler : IRequestHandler<UpdateStoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _datetime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileUploader _fileUploader;
        public UpdateStoryCommandHandler(IApplicationDbContext context, IDateTime dateTime,
            ICurrentUserService currentUserService, IFileUploader fileUploader
            )
        {
            _context = context;
            _datetime = dateTime;
            _currentUserService = currentUserService;
            _fileUploader = fileUploader;
        }

        public async Task<Unit> Handle(UpdateStoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Stories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Story), request.Id);
            }

            // Story could be only modified by its author/creator
            if (entity.AuthorId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }



            entity.SetTitle(request.Title);
            entity.SetDescription(request.Description);
            entity.SetContent(request.Content);
            //Needs Setter
            entity.GenreId = request.GenreId;


            if (request.Photo?.Length > 0)
            {
                entity.PhotoPath = await _fileUploader.UploadCoverPhotoAsync(request.Photo);
            }
            if (request.Thumbnail?.Length > 0)
            {
                entity.ThumbnailPath = await _fileUploader.UploadThumbnailAsync(request.Thumbnail);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}