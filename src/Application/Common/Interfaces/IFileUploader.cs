using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Common.Interfaces
{
    public interface IFileUploader
    {
        Task<string> UploadCoverPhotoAsync(byte[] photo);
        Task<string> UploadThumbnailAsync(byte[] thumbnail);
    }
}
