using MeowStory.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Domain.Entities
{
    public class Comment : BaseEntity, IHasDomainEvent
    {
        public Story Story { get; set; }
        public string StoryId { get; set; }
        public string Text { get; set; }
        public UserProfile User { get; set; }
        public string UserId { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
