using MeowStory.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Infrastructure.Files
{
    public class FileUploader : IFileUploader
    {
        public Task<string> UploadCoverPhotoAsync(byte[] photo)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadThumbnailAsync(byte[] thumbnail)
        {
            throw new NotImplementedException();
        }
    }
}