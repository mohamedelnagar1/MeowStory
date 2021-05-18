using AutoMapper;
using MeowStory.Application.Authors.Queries;
using MeowStory.Application.Common.Mappings;
using MeowStory.Application.Genres.Queries;
using MeowStory.Domain.Entities;
using System;

namespace MeowStory.Application.Stories.Queries
{
    public class StoryDto : IMapFrom<Story>
    {
        public string Id { get; set; }
        public AuthorDto Author { get; set; }
        public string Title { get; set; }
        public GenreDto Genre { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string ThumbnailPath { get; set; }
        public int Votes { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }

    }
}
