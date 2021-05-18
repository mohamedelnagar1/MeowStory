using MeowStory.Application.Common.Mappings;
using MeowStory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Stories.Queries
{
    public class CommentDto : IMapFrom<Comment>
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public UserProfile User { get; set; }
    }
}