using MeowStory.Domain.Common;
using MeowStory.Domain.Entities;

namespace MeowStory.Domain.Events
{
    public class CommentAddedEvent : DomainEvent
    {
        public CommentAddedEvent(Comment comment)
        {
            Comment = comment;
        }

        public Comment Comment { get; private set; }
    }
}
