using MeowStory.Application.Stories.Queries.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Common.Interfaces
{

    public interface IPdfFileBuilder
    {
        byte[] BuildStoryDocument(StoryExportDto exportDto);
    }
}
