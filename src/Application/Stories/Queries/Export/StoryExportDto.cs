using AutoMapper;
using MeowStory.Application.Authors.Queries;
using MeowStory.Application.Common.Mappings;
using MeowStory.Domain.Entities;
using System;

namespace MeowStory.Application.Stories.Queries.Export
{
    public class StoryExportDto : IMapFrom<Story>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string Content { get; set; }
        public AuthorDto Author { get; set; }
        public DateTime? DatePublished { get; set; }


    }
}