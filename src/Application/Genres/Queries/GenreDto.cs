using MeowStory.Application.Common.Mappings;
using MeowStory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Genres.Queries
{
    public class GenreDto : IMapFrom<Genre>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}