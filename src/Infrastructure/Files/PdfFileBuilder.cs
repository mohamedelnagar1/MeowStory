using MeowStory.Application.Common.Interfaces;
using MeowStory.Application.Stories.Queries.Export;
using System;

namespace MeowStory.Infrastructure.Files
{
    public class PdfFileBuilder : IPdfFileBuilder
    {
        public byte[] BuildStoryDocument(StoryExportDto exportDto)
        {
            //TODO: Implement Pdf Builder using DinKPdf
            throw new NotImplementedException();
        }

    }
}