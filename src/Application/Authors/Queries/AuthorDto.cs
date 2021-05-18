using MeowStory.Application.Common.Mappings;
using MeowStory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Authors.Queries
{
    public class AuthorDto : IMapFrom<Author>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
